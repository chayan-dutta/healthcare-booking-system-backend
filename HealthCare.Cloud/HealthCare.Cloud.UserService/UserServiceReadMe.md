## 🎯 **Goal of UserService**

Manage all user-related data and operations, such as:

* Patients
* Doctors (basic identity; full profile managed in DoctorService)
* Hospital Admins
* Super Admins

This service **does not handle login/auth** — that's the responsibility of `AuthService`. Instead, it stores and serves **user profile data** and handles **role- and tenant-specific logic**.

---

## 🧱 1. Service Responsibilities

### ✅ Core Responsibilities:

* Store user profiles and metadata
* Assign and manage user roles
* Track hospital association (for doctors and admins)
* Track new user status (for first-time discount eligibility)
* Provide APIs to fetch user info based on JWT claims
* Update personal information (e.g., name, phone)

### ❌ Excluded (handled elsewhere):

* Authentication (AuthService)
* Passwords
* Tokens
* Permissions beyond basic roles

---

## 🧩 2. Entity Design

### 🔸 `User` Entity (Core Fields)

| Field                | Type     | Description                                    |
| -------------------- | -------- | ---------------------------------------------- |
| `Id`                 | GUID     | Primary key                                    |
| `Email`              | string   | Email, synced from AuthService                 |
| `FullName`           | string   | User’s display name                            |
| `Role`               | enum     | Patient, Doctor, HospitalAdmin, SuperAdmin     |
| `HospitalId`         | GUID?    | Null for patients, required for doctors/admins |
| `PhoneNumber`        | string   | Optional                                       |
| `IsFirstAppointment` | bool     | Used for Z% discount logic                     |
| `CreatedAt`          | DateTime | Timestamp                                      |
| `Gender`		       | enum     | Gender                                      |
| `DOB`				   | DateTime | Timestamp                                      |
| `Address`		       | jsonb    | address                                      |


---

## 🔀 3. Communication with Other Services

* **AuthService → UserService**: On successful registration, AuthService calls UserService to create a user profile.
* **DoctorService → UserService**: Fetch doctor-user info by `UserId` (name/email).
* **AppointmentService → UserService**: Read-only lookup of user profiles.
* **PaymentService → UserService**: To check if user is eligible for first-time discount.

👉 Use **REST endpoints or internal messaging (optional)** for this.

---

## 📂 4. Suggested APIs (REST)

| Endpoint                            | Method | Purpose                             |
| ----------------------------------- | ------ | ----------------------------------- |
| `/api/users/me`                     | GET    | Get current user profile from token |
| `/api/users/{id}`                   | GET    | Get user by ID                      |
| `/api/users`                        | POST   | Create user (called by AuthService) |
| `/api/users/{id}`                   | PUT    | Update profile                      |
| `/api/users/{id}/role`              | PATCH  | Change role (admin-only)            |
| `/api/users/{id}/hospital`          | PATCH  | Assign to hospital                  |
| `/api/users/{id}/first-appointment` | PATCH  | Mark after first appointment        |

---

## 🛡️ 5. Role & Access Control

Handled via JWT claims and API Gateway:

* Check role in token (`User.Role == "Doctor"`)
* Restrict access to endpoints based on role
* Forward token claims to downstream services (like `UserService`)

---

## 🗃️ 6. Database & Persistence

* DB: PostgreSQL or any RDBMS
* One `Users` table with basic user info
* Optional: add `AuditLogs` table for profile updates
* Use soft deletes or status flags (e.g., `IsActive`, `IsDeleted`)

---

## 📦 7. Deployment & DevOps

* Containerize the service (Dockerfile)
* Expose internal API endpoint to other services
* Add basic health check (`/health`)
* Use logging (Serilog or built-in .NET logger)
* Register with service discovery (e.g., Consul) or DNS-based routing

---

## 📈 8. Future Enhancements

* Avatar/photo upload (via blob storage)
* GDPR-style data deletion
* 2FA preferences and user settings
* Multi-lingual support (name, preferences, notifications)

---

Let me know when you're ready to:

* **Design the API contracts** (request/response bodies)
* **Move to database modeling**
* Or plug it into the rest of the system

Would you like to tackle **AuthService next**, or wire up this UserService with dummy data first?
