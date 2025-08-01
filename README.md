# healthcare-booking-system

## ✅ Basic Functional Requirements

### 👥 User & Roles

* Patients can register, log in, and manage their profile.
* Doctors can register or be added by Admin, manage availability, and view appointments.
* Admin can manage users, doctors, and view system analytics.

### 🔐 Authentication & Authorization

* User registration and login (via email & password)
* Role-based access control (Patient, Doctor, Admin)
* JWT token issuance and validation

### 🩺 Doctor Management

* View doctor profiles (name, specialization, availability)
* Search/filter doctors (by specialization, city, etc.)
* Manage doctor availability (add/edit/delete time slots)

### 📆 Appointment Management

* Book appointments with available doctors
* View upcoming and past appointments
* Cancel/reschedule appointments
* Doctor can accept/reject appointments (optional)

### 🔔 Notifications

* Send email/SMS/in-app notifications on:

  * Appointment booking
  * Appointment rescheduling/cancellation
  * Reminders before appointment
* Track sent notifications

### 📊 Admin Dashboard (later, optional)

* View user statistics
* Doctor onboarding/approval
* System logs or audit trail

---

## ✅ Basic Non-Functional Requirements

### ⚙️ Architecture

* Microservice-based (each service has its own DB)
* Containerized using Docker
* Internal communication via REST (initially), RabbitMQ (later)
* API Gateway for routing

### 🛡️ Security

* Secure password storage (hashing via BCrypt)
* Token expiration and refresh handling (optional)
* HTTPS enforced

### 📈 Scalability & Performance

* Services should scale independently
* Async operations (notifications, logs)
* Caching for frequent queries (doctor list)

### 🔍 Logging & Monitoring

* Centralized logging (e.g., Serilog + Seq/ELK)
* Health checks for each service
* Metrics with Prometheus/Grafana

### 📦 DevOps

* Docker Compose for local dev
* Kubernetes manifests for deployment (optional)
* CI/CD pipeline for builds & deployment

---

## 🧩 Microservices Breakdown (Initial Scope)

| Microservice            | Key Features                                  |
| ----------------------- | --------------------------------------------- |
| **AuthService**         | Register/Login, JWT issuance, role management |
| **UserService**         | Patient/Doctor profiles, user info            |
| **DoctorService**       | Doctor profiles, availability, search         |
| **AppointmentService**  | Book/reschedule/cancel appointments           |
| **NotificationService** | Send email/SMS reminders (via RabbitMQ)       |
| **ApiGateway**          | Single entry point, routes to services        |

---
