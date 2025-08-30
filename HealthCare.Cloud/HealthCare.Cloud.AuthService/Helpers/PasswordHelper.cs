using System.Security.Cryptography;

namespace HealthCare.Cloud.AuthService.Helpers;

/// <summary>
/// Utility class for securely hashing and verifying passwords using PBKDF2.
/// 
/// How password validation works:
/// 1. During registration, we generate:
///    - A random salt (unique for each user)
///    - A password hash derived using PBKDF2 (with the salt + password)
///    - We store BOTH salt and hash in the database (never the plain password).
/// 
/// 2. During login, we:
///    - Retrieve the stored salt and hash from the database
///    - Recompute the hash using the entered password and the same salt
///    - Compare recomputed hash with stored hash using constant-time comparison
///    - If they match → password is valid, otherwise → invalid
/// </summary>
public static class PasswordHelper
{
    // =============================
    // CONSTANTS for consistency
    // =============================

    private const int SaltSize = 32;                // 32 bytes = 256 bits, secure random salt size
    private const int HashSize = 32;                // 32 bytes = 256 bits output for SHA256
    private const int Iterations = 100_000;         // PBKDF2 iterations (higher = stronger but slower)
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256; // Algorithm choice

    // =============================
    // HASH GENERATION
    // =============================

    /// <summary>
    /// Generates a secure password hash and salt for storing in the database.
    /// </summary>
    /// <param name="password">Plain-text password entered by the user</param>
    /// <returns>Tuple containing hash and salt (both byte arrays)</returns>
    public static (byte[] hash, byte[] salt) GeneratePasswordHash(string password)
    {
        // STEP 1: Generate a secure random salt
        using var rng = RandomNumberGenerator.Create();  // Cryptographically secure RNG
        byte[] salt = new byte[SaltSize];                // Allocate memory for salt
        rng.GetBytes(salt);                              // Fill salt with random values

        // STEP 2: Generate hash using PBKDF2
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,                                    // Plain-text password
            salt,                                        // Unique salt for this user
            Iterations,                                  // Iteration count for security
            Algorithm                                    // Chosen hash algorithm
        );

        byte[] hash = pbkdf2.GetBytes(HashSize);         // Derive final password hash

        // STEP 3: Return both hash & salt for DB storage
        return (hash, salt);
    }

    // =============================
    // PASSWORD VERIFICATION
    // =============================

    /// <summary>
    /// Verifies if the provided password matches the stored hash + salt.
    /// </summary>
    /// <param name="password">Plain-text password entered by the user during login</param>
    /// <param name="storedHash">The hash stored in the database (from registration)</param>
    /// <param name="storedSalt">The salt stored in the database (from registration)</param>
    /// <returns>True if password is valid, otherwise false</returns>
    public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        // STEP 1: Recompute hash with same parameters
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,                                    // User's entered password
            storedSalt,                                  // Salt retrieved from DB
            Iterations,                                  // Must match stored parameters
            Algorithm                                    // Must match stored parameters
        );

        byte[] computedHash = pbkdf2.GetBytes(HashSize); // Recomputed hash

        // STEP 2: Compare recomputed hash with stored hash in constant time
        // (Prevents timing attacks by ensuring comparison takes same time regardless of result)
        return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
    }
}
