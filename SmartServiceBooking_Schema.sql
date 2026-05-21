/*
    Smart Service Booking Platform - Database Schema
    Target: SQL Server
    Author: Antigravity
    Date: 2025-12-16
*/

-- Enable containment if needed, or just standard DB settings
-- CREATE DATABASE [SmartServiceBookingDB];
-- GO
-- USE [SmartServiceBookingDB];
-- GO

-- create schemas
CREATE SCHEMA [Identity];
GO
CREATE SCHEMA [Common];
GO
CREATE SCHEMA [Service];
GO
CREATE SCHEMA [Booking];
GO
CREATE SCHEMA [Payment];
GO
CREATE SCHEMA [Interaction];
GO
CREATE SCHEMA [System];
GO

-- =============================================
-- System & Audit Module
-- =============================================
CREATE TABLE [System].[AuditLogs] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [UserId] NVARCHAR(450) NULL, -- FK to Users later
    [EntityName] NVARCHAR(100) NOT NULL,
    [EntityId] NVARCHAR(450) NOT NULL,
    [Action] NVARCHAR(50) NOT NULL, -- Create, Update, SoftDelete, Delete
    [OldValues] NVARCHAR(MAX) NULL, -- JSON
    [NewValues] NVARCHAR(MAX) NULL, -- JSON
    [Timestamp] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

CREATE TABLE [System].[AppSettings] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Key] NVARCHAR(100) NOT NULL UNIQUE,
    [Value] NVARCHAR(MAX) NOT NULL,
    [Description] NVARCHAR(255) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt] DATETIME2 NULL
);
GO

-- =============================================
-- Identity & Users Module
-- =============================================
CREATE TABLE [Identity].[Roles] (
    [Id] NVARCHAR(450) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(256) NULL,
    [NormalizedName] NVARCHAR(256) NULL,
    [ConcurrencyStamp] NVARCHAR(MAX) NULL,
    [Description] NVARCHAR(255) NULL
);
GO
CREATE UNIQUE INDEX [RoleNameIndex] ON [Identity].[Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE TABLE [Identity].[Users] (
    [Id] NVARCHAR(450) NOT NULL PRIMARY KEY,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
    [UserName] NVARCHAR(256) NULL,
    [NormalizedUserName] NVARCHAR(256) NULL,
    [Email] NVARCHAR(256) NULL,
    [NormalizedEmail] NVARCHAR(256) NULL,
    [EmailConfirmed] BIT NOT NULL DEFAULT 0,
    [PasswordHash] NVARCHAR(MAX) NULL,
    [SecurityStamp] NVARCHAR(MAX) NULL,
    [ConcurrencyStamp] NVARCHAR(MAX) NULL,
    [PhoneNumber] NVARCHAR(50) NULL,
    [PhoneNumberConfirmed] BIT NOT NULL DEFAULT 0,
    [TwoFactorEnabled] BIT NOT NULL DEFAULT 0,
    [LockoutEnd] DATETIMEOFFSET NULL,
    [LockoutEnabled] BIT NOT NULL DEFAULT 0,
    [AccessFailedCount] INT NOT NULL DEFAULT 0,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [ProfilePictureUrl] NVARCHAR(500) NULL,
    [LastLoginDate] DATETIME2 NULL,
    
    -- Audit
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [CreatedBy] NVARCHAR(450) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(450) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0
);
GO
CREATE INDEX [IX_Users_NormalizedUserName] ON [Identity].[Users] ([NormalizedUserName]);
CREATE UNIQUE INDEX [IX_Users_NormalizedEmail] ON [Identity].[Users] ([NormalizedEmail]) WHERE [NormalizedEmail] IS NOT NULL;
CREATE INDEX [IX_Users_PhoneNumber] ON [Identity].[Users] ([PhoneNumber]);
GO

CREATE TABLE [Identity].[UserRoles] (
    [UserId] NVARCHAR(450) NOT NULL,
    [RoleId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Identity].[UserTokens] (
    [UserId] NVARCHAR(450) NOT NULL,
    [LoginProvider] NVARCHAR(450) NOT NULL,
    [Name] NVARCHAR(450) NOT NULL,
    [Value] NVARCHAR(MAX) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE CASCADE
);
GO

-- =============================================
-- Locations Module
-- =============================================
CREATE TABLE [Common].[Countries] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,
    [ISOCode] CHAR(2) NOT NULL,
    [PhoneCode] NVARCHAR(10) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE [Common].[Cities] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [CountryId] INT NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [FK_Cities_Countries] FOREIGN KEY ([CountryId]) REFERENCES [Common].[Countries] ([Id])
);
GO

CREATE TABLE [Common].[Addresses] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [UserId] NVARCHAR(450) NULL, -- Can be null for provider generic locations
    [Title] NVARCHAR(50) NULL, -- Home, Work
    [CityId] INT NOT NULL,
    [Street] NVARCHAR(200) NOT NULL,
    [Building] NVARCHAR(50) NULL,
    [Latitude] DECIMAL(9, 6) NULL,
    [Longitude] DECIMAL(9, 6) NULL,
    
    -- Audit
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [CreatedBy] NVARCHAR(450) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(450) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_Addresses_Users] FOREIGN KEY ([UserId]) REFERENCES [Identity].[Users] ([Id]), -- No Cascade
    CONSTRAINT [FK_Addresses_Cities] FOREIGN KEY ([CityId]) REFERENCES [Common].[Cities] ([Id])
);
GO

-- =============================================
-- Services & Providers Module
-- =============================================
CREATE TABLE [Service].[Categories] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ParentId] INT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [IconUrl] NVARCHAR(500) NULL,
    [Path] NVARCHAR(500) NULL, -- Materialized Path '1/5/10'
    [Level] INT NOT NULL DEFAULT 0,
    [IsActive] BIT NOT NULL DEFAULT 1,
    
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [CreatedBy] NVARCHAR(450) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(450) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_Categories_Parent] FOREIGN KEY ([ParentId]) REFERENCES [Service].[Categories] ([Id]) -- No Cascade
);
GO

CREATE TABLE [Service].[Services] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [CategoryId] INT NOT NULL,
    [Name] NVARCHAR(200) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [BasePrice] DECIMAL(18,2) NOT NULL,
    [PriceUnit] NVARCHAR(20) NOT NULL, -- 'Fixed', 'Hourly'
    [IsActive] BIT NOT NULL DEFAULT 1,

    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [CreatedBy] NVARCHAR(450) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(450) NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_Services_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Service].[Categories] ([Id])
);
GO

CREATE TABLE [Service].[ServiceProviders] (
    [Id] NVARCHAR(450) NOT NULL PRIMARY KEY, -- Same as User Id
    [BusinessName] NVARCHAR(200) NULL,
    [Bio] NVARCHAR(MAX) NULL,
    [Rating] DECIMAL(3, 2) NOT NULL DEFAULT 0,
    [ReviewCount] INT NOT NULL DEFAULT 0,
    [IsVerified] BIT NOT NULL DEFAULT 0,
    [CommissionRate] DECIMAL(5, 2) NOT NULL DEFAULT 0, -- Percentage

    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_ServiceProviders_Users] FOREIGN KEY ([Id]) REFERENCES [Identity].[Users] ([Id])
);
GO

CREATE TABLE [Service].[ProviderServices] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [ProviderId] NVARCHAR(450) NOT NULL,
    [ServiceId] BIGINT NOT NULL,
    [CustomPrice] DECIMAL(18,2) NULL, -- Overrides BasePrice
    [IsActive] BIT NOT NULL DEFAULT 1,

    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_ProviderServices_Providers] FOREIGN KEY ([ProviderId]) REFERENCES [Service].[ServiceProviders] ([Id]),
    CONSTRAINT [FK_ProviderServices_Services] FOREIGN KEY ([ServiceId]) REFERENCES [Service].[Services] ([Id]),
    CONSTRAINT [UQ_Provider_Service] UNIQUE ([ProviderId], [ServiceId])
);
GO

CREATE TABLE [Service].[ProviderAvailability] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [ProviderId] NVARCHAR(450) NOT NULL,
    [DayOfWeek] INT NOT NULL, -- 0=Sunday, 6=Saturday
    [StartTime] TIME NOT NULL,
    [EndTime] TIME NOT NULL,
    
    CONSTRAINT [FK_ProviderAvailability_Providers] FOREIGN KEY ([ProviderId]) REFERENCES [Service].[ServiceProviders] ([Id])
);
GO

-- =============================================
-- Booking Module
-- =============================================
CREATE TABLE [Booking].[Bookings] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [CustomerId] NVARCHAR(450) NOT NULL,
    [ProviderId] NVARCHAR(450) NOT NULL,
    [ServiceId] BIGINT NOT NULL,
    [LocationId] BIGINT NOT NULL,
    
    [BookingStatus] NVARCHAR(50) NOT NULL, -- Pending, Confirmed, Completed, Cancelled
    [ScheduledStart] DATETIME2 NOT NULL,
    [ScheduledEnd] DATETIME2 NOT NULL,
    [TotalAmount] DECIMAL(18,2) NOT NULL,
    [Notes] NVARCHAR(500) NULL,
    
    [RowVersion] ROWVERSION, -- Optimistic Concurrency

    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [CreatedBy] NVARCHAR(450) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] NVARCHAR(450) NULL,
    [CancelledAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_Bookings_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Identity].[Users] ([Id]), -- No Cascade
    CONSTRAINT [FK_Bookings_Provider] FOREIGN KEY ([ProviderId]) REFERENCES [Service].[ServiceProviders] ([Id]),
    CONSTRAINT [FK_Bookings_Service] FOREIGN KEY ([ServiceId]) REFERENCES [Service].[Services] ([Id]),
    CONSTRAINT [FK_Bookings_Location] FOREIGN KEY ([LocationId]) REFERENCES [Common].[Addresses] ([Id])
);
GO

CREATE TABLE [Booking].[BookingStatusHistory] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [BookingId] BIGINT NOT NULL,
    [PreviousStatus] NVARCHAR(50) NULL,
    [Status] NVARCHAR(50) NOT NULL,
    [Reason] NVARCHAR(500) NULL,
    [ChangedBy] NVARCHAR(450) NULL,
    [ChangedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    
    CONSTRAINT [FK_BookingStatusHistory_Bookings] FOREIGN KEY ([BookingId]) REFERENCES [Booking].[Bookings] ([Id])
);
GO

-- =============================================
-- Payment Module
-- =============================================
CREATE TABLE [Payment].[Wallets] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [UserId] NVARCHAR(450) NOT NULL,
    [Balance] DECIMAL(18,2) NOT NULL DEFAULT 0,
    [Currency] CHAR(3) NOT NULL DEFAULT 'USD',
    [IsLocked] BIT NOT NULL DEFAULT 0,
    [RowVersion] ROWVERSION,

    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt] DATETIME2 NULL,

    CONSTRAINT [FK_Wallets_Users] FOREIGN KEY ([UserId]) REFERENCES [Identity].[Users] ([Id])
);
GO

CREATE TABLE [Payment].[Transactions] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [WalletId] BIGINT NOT NULL,
    [BookingId] BIGINT NULL,
    [Amount] DECIMAL(18,2) NOT NULL,
    [Type] NVARCHAR(20) NOT NULL, -- Credit, Debit
    [ReferenceId] NVARCHAR(100) NULL, -- Payment Gateway ID
    [Status] NVARCHAR(20) NOT NULL, -- Pending, Success, Failed
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    
    CONSTRAINT [FK_Transactions_Wallets] FOREIGN KEY ([WalletId]) REFERENCES [Payment].[Wallets] ([Id]),
    CONSTRAINT [FK_Transactions_Bookings] FOREIGN KEY ([BookingId]) REFERENCES [Booking].[Bookings] ([Id])
);
GO
CREATE INDEX [IX_Transactions_Wallet_Booking] ON [Payment].[Transactions] ([WalletId], [BookingId], [CreatedAt]);
GO

CREATE TABLE [Payment].[Invoices] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [BookingId] BIGINT NOT NULL,
    [InvoiceNumber] NVARCHAR(50) NOT NULL UNIQUE,
    [Amount] DECIMAL(18,2) NOT NULL,
    [IssuedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [DueDate] DATETIME2 NOT NULL,
    [PaidAt] DATETIME2 NULL,
    [PdfUrl] NVARCHAR(500) NULL,

    CONSTRAINT [FK_Invoices_Bookings] FOREIGN KEY ([BookingId]) REFERENCES [Booking].[Bookings] ([Id])
);
GO

CREATE TABLE [Payment].[PaymentMethods] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [UserId] NVARCHAR(450) NOT NULL,
    [Provider] NVARCHAR(50) NOT NULL, -- 'Stripe', 'PayPal'
    [CardLast4] CHAR(4) NULL,
    [Token] NVARCHAR(MAX) NOT NULL,
    [IsDefault] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [FK_PaymentMethods_Users] FOREIGN KEY ([UserId]) REFERENCES [Identity].[Users] ([Id])
);
GO

-- =============================================
-- Reviews & Notifications Module
-- =============================================
CREATE TABLE [Interaction].[Reviews] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [BookingId] BIGINT NOT NULL,
    [ReviewerId] NVARCHAR(450) NOT NULL,
    [RateeId] NVARCHAR(450) NULL, -- Could be Provider or Customer
    [Rating] INT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    [Comment] NVARCHAR(1000) NULL,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAt] DATETIME2 NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    CONSTRAINT [FK_Reviews_Booking] FOREIGN KEY ([BookingId]) REFERENCES [Booking].[Bookings] ([Id]),
    CONSTRAINT [FK_Reviews_Reviewer] FOREIGN KEY ([ReviewerId]) REFERENCES [Identity].[Users] ([Id]),
    CONSTRAINT [FK_Reviews_Ratee] FOREIGN KEY ([RateeId]) REFERENCES [Identity].[Users] ([Id]),
    CONSTRAINT [UQ_Booking_Reviewer] UNIQUE ([BookingId], [ReviewerId])
);
GO

CREATE TABLE [Interaction].[Notifications] (
    [Id] BIGINT IDENTITY(1,1) PRIMARY KEY,
    [UserId] NVARCHAR(450) NOT NULL,
    [Title] NVARCHAR(200) NOT NULL,
    [Message] NVARCHAR(MAX) NOT NULL,
    [Type] NVARCHAR(50) NOT NULL, -- 'Info', 'Warning', 'BookingUpdate'
    [ReferenceType] NVARCHAR(50) NULL, -- 'Booking', 'Review'
    [ReferenceId] NVARCHAR(450) NULL,
    [IsRead] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT [FK_Notifications_Users] FOREIGN KEY ([UserId]) REFERENCES [Identity].[Users] ([Id])
);
GO
