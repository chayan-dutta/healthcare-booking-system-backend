
-- Table: public.Users

-- DROP TABLE IF EXISTS public."Users";

CREATE TABLE IF NOT EXISTS public."Users"
(
    "Id" uuid NOT NULL,
    "Email" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "FullName" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Role" integer NOT NULL,
    "HospitalId" uuid,
    "PhoneNumber" character varying(20) COLLATE pg_catalog."default",
    "IsFirstAppointment" boolean NOT NULL,
    "CreatedAt" timestamp without time zone NOT NULL,
    "Gender" integer NOT NULL,
    "DOB" date NOT NULL,
    "Address" jsonb,
    "UpdatedAt" timestamp without time zone,
    "IsActive" integer NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."Users"
    OWNER to postgres;
-- Index: IX_Users_Email

-- DROP INDEX IF EXISTS public."IX_Users_Email";

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Users_Email"
    ON public."Users" USING btree
    ("Email" COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
