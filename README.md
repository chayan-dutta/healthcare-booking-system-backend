# healthcare-booking-system

## ✅ **Functional Requirements (Updated)**

### 👥 User & Roles

* **Patients**

  * Can self-register
  * View/search doctors from any hospital
  * Book, reschedule, or cancel appointments

* **Doctors**

  * Cannot self-register as verified doctors
  * Must be added/invited by a **Hospital Admin**
  * Can manage their availability and view their appointments
  * Must be verified before allowed to serve

* **Hospital Admins**

  * Belong to a specific hospital
  * Can:

    * Add/verify doctors within their hospital
    * View/manage appointments of doctors in their hospital
    * Manage hospital details

* **(Optional) Super Admin**

  * Can:

    * Manage hospitals and their admins
    * View global analytics
    * Approve hospital registrations

---

### 🏥 Hospital Management (Multi-Tenancy)

* Each hospital:

  * Has its own identity, name, contact details
  * Can register/login via hospital admin
  * Has its own list of doctors and statistics
* Each doctor belongs to one hospital (`HospitalId`)
* Data visibility is scoped:

  * Hospital Admins see only their hospital's data
  * Patients see doctors across all hospitals

---

### 🔐 Authentication & Authorization

* Secure registration/login using email + password
* Role-based access (`Patient`, `Doctor`, `HospitalAdmin`, `SuperAdmin`)
* JWT token issuance and role-based access control
* Optional: email verification or 2FA support

---

### 🩺 Doctor Management

* Hospital admins can:

  * Add or invite doctors
  * Review and verify submitted doctor credentials (e.g., license, ID)
  * Suspend or remove doctors
* Doctors can:

  * Edit their profile and availability
  * View their upcoming/past appointments
* Search doctors by:

  * Hospital
  * Specialization
  * City
  * Availability

---

### 📆 Appointment Management

* Patients can:

  * Browse available time slots
  * Book, reschedule, or cancel appointments
* Doctors can:

  * Accept or reject appointments (optional)
* Admins can:

  * View/manage appointments of their hospital
* Statuses: `Pending`, `Confirmed`, `Cancelled`, `Rejected`

---

### 🔔 Notifications

* Notification triggers:

  * Appointment booked, rescheduled, or cancelled
  * Reminders before appointment (e.g., 24hr, 1hr)
* Channels:

  * Email
  * SMS (optional)
  * In-app (for doctors/admins)
* Track delivery status for all notifications

---

### 📊 Admin Dashboard (Optional, Phase 2)

* View metrics:

  * Total patients
  * Number of appointments (daily/weekly/monthly)
  * Doctor performance (appointment load)
* Approve/reject hospital or doctor registrations
* View audit logs and login activity

## 💰 Monetization & Payment Flow (Updated Functional Requirements)
### 🏥 Hospital Subscription (Platform Revenue)
Hospitals must pay X (one-time or recurring) to:
- Register on the platform
- Get verified and onboarded
- Gain access to doctor and appointment management
- Payment triggers verification workflow (manual or automated)
- Super Admin approves after payment and document review

## 👨‍⚕️ Doctor Consultation Charges (Hospital Revenue)
Each hospital can define its own pricing model:
- Set a consultation/appointment fee (₹Y)
- Optionally, define separate rates for different doctors
- Patient is shown the final payable amount at booking time
- Payment gateway integration required (e.g., Razorpay, Stripe)

## 🎁 First-Time User Discount (Platform Promo)
New patients receive a Z% discount (e.g., 50%) on their first appointment
- System tracks if IsFirstAppointment == true for the user
- Discount is applied at checkout
- Platform can optionally subsidize the discount (split fee with hospital)
---

## ✅ **Non-Functional Requirements (Updated)**

### ⚙️ Architecture

* Microservice-based design with clear domain boundaries
* Each service has its own database (Database-per-service)
* Internal sync communication: REST (initial)
* Internal async communication: RabbitMQ (for notifications)
* API Gateway for routing, rate limiting, and access control

---

### 🛡️ Security

* Enforce HTTPS
* Secure password hashing (BCrypt or PBKDF2)
* JWT-based access tokens with role and tenant (HospitalId) claims
* Optional:

  * Token refresh mechanism
  * IP allowlisting for Hospital Admins
  * Secure document storage (S3, Azure Blob) for doctor verification docs

---

### 📈 Scalability & Performance

* Independent scalability of services
* Support horizontal scaling of notification service
* Use caching for high-read endpoints (doctor list, slots)
* Pagination and filtering for large result sets (appointments, users)

---

### 🔍 Logging & Monitoring

* Centralized structured logging (e.g., Serilog + Seq / ELK)
* Log:

  * User activity (logins, changes)
  * Appointment actions
  * Errors and exceptions
* Health checks (liveness/readiness probes)
* Metrics collection with Prometheus + Grafana

---

### 📦 DevOps & Deployment

* **Local Dev:** Docker Compose with all services + PostgreSQL + RabbitMQ
* **Deployment:** Kubernetes (Helm or Kustomize manifests)
* **CI/CD:**

  * GitHub Actions or Azure DevOps for:

    * Build and push Docker images
    * Deploy to cluster with zero downtime
* **Environment Config:**

  * Use environment variables or centralized config service (Consul/etcd)

---

## 🧩 Microservices Breakdown (Updated)

| Microservice            | Key Features                                                    |
| ----------------------- | --------------------------------------------------------------- |
| **AuthService**         | User login/registration, JWT handling, role enforcement         |
| **UserService**         | Profile management, hospital/role lookup                        |
| **HospitalService**     | Manage hospital info, register hospital, assign hospital admins |
| **DoctorService**       | Manage doctor data, verification, availability                  |
| **AppointmentService**  | Booking/rescheduling/canceling appointments                     |
| **NotificationService** | Email/SMS reminders via RabbitMQ                                |
| **ApiGateway**          | Single entry point, role-based routing, token verification      |

---
