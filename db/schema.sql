CREATE DATABASE IF NOT EXISTS vehicle_finder;

USE vehicle_finder;

-- =========================================
-- FINANCE COMPANIES
-- =========================================

CREATE TABLE finances (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);


-- =========================================
-- BRANCHES
-- =========================================

CREATE TABLE branches (
    id INT AUTO_INCREMENT PRIMARY KEY,
    finance_id INT NOT NULL,
    name VARCHAR(100) NOT NULL,
    city VARCHAR(100) NOT NULL,

    CONSTRAINT fk_branch_finance
        FOREIGN KEY (finance_id)
        REFERENCES finances(id)
        ON DELETE CASCADE
);


-- =========================================
-- VEHICLE RECORDS
-- =========================================

CREATE TABLE vehicle_records (
    id INT AUTO_INCREMENT PRIMARY KEY,

    branch_id INT NOT NULL,

    vehicle_no VARCHAR(20) NOT NULL,
    chassis_no VARCHAR(17) NOT NULL,
    engine_no VARCHAR(30) NOT NULL,

    model VARCHAR(100) NOT NULL,

    customer_name VARCHAR(100) NOT NULL,
    customer_mobile VARCHAR(15) NOT NULL,

    overdue_amount DECIMAL(12,2) NOT NULL,

    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    -- Last 4 characters of vehicle number
    vehicle_last4 VARCHAR(4)
        GENERATED ALWAYS AS (RIGHT(vehicle_no, 4)) STORED,

    -- Last 5 characters of chassis number
    chassis_last5 VARCHAR(5)
        GENERATED ALWAYS AS (RIGHT(chassis_no, 5)) STORED,

    CONSTRAINT fk_vehicle_branch
        FOREIGN KEY (branch_id)
        REFERENCES branches(id)
        ON DELETE CASCADE
);


-- =========================================
-- INDEXES FOR FAST SEARCH
-- =========================================

CREATE INDEX idx_vehicle_last4
ON vehicle_records(vehicle_last4);

CREATE INDEX idx_chassis_last5
ON vehicle_records(chassis_last5);


-- =========================================
-- SEARCH LOGS
-- =========================================

CREATE TABLE search_logs (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,

    query VARCHAR(10) NOT NULL,

    search_type ENUM('rc', 'chassis') NOT NULL,

    result_count INT NOT NULL,

    searched_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);