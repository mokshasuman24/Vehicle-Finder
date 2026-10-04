USE vehicle_finder;

-- =========================================================
-- VEHICLE FINDER - SEED DATA
-- =========================================================
-- This script is intended to be run after schema.sql.
-- It creates:
--   5 finance companies
--   15 branches
--   50,000 vehicle records
--
-- Do NOT run this again on an already seeded database,
-- otherwise duplicate records will be created.
-- =========================================================


-- =========================================================
-- 1. FINANCE COMPANIES
-- =========================================================

INSERT INTO finances (name)
VALUES
('HDFC Bank'),
('Bajaj Finance'),
('ICICI Bank'),
('Axis Bank'),
('Mahindra Finance');


-- =========================================================
-- 2. BRANCHES
-- =========================================================

INSERT INTO branches (finance_id, name, city)
VALUES
(1, 'Jaipur Main', 'Jaipur'),
(1, 'Jodhpur Branch', 'Jodhpur'),
(1, 'Udaipur Branch', 'Udaipur'),

(2, 'Bangalore Main', 'Bangalore'),
(2, 'Mysore Branch', 'Mysore'),
(2, 'Hubli Branch', 'Hubli'),

(3, 'Mumbai Main', 'Mumbai'),
(3, 'Pune Branch', 'Pune'),
(3, 'Nagpur Branch', 'Nagpur'),

(4, 'Delhi Main', 'Delhi'),
(4, 'Noida Branch', 'Noida'),
(4, 'Gurgaon Branch', 'Gurgaon'),

(5, 'Chennai Main', 'Chennai'),
(5, 'Coimbatore Branch', 'Coimbatore'),
(5, 'Madurai Branch', 'Madurai');


-- =========================================================
-- 3. GENERATE 50,000 VEHICLE RECORDS
-- =========================================================

SET SESSION cte_max_recursion_depth = 60000;

INSERT INTO vehicle_records
(
    branch_id,
    vehicle_no,
    chassis_no,
    engine_no,
    model,
    customer_name,
    customer_mobile,
    overdue_amount
)

WITH RECURSIVE numbers AS
(
    SELECT 1 AS n

    UNION ALL

    SELECT n + 1
    FROM numbers
    WHERE n < 50000
)

SELECT

    -- Branch IDs 1 to 15
    1 + MOD(n - 1, 15) AS branch_id,


    -- Real-looking Indian vehicle registration number
    CONCAT(

        ELT(
            1 + MOD(n - 1, 5),

            'RJ14',
            'MH12',
            'DL3',
            'KA05',
            'TN38'

        ),

        ELT(
            1 + MOD(n - 1, 4),

            'CA',
            'AB',
            'CB',
            'DE'

        ),

        LPAD(
            MOD(n - 1, 10000),
            4,
            '0'
        )

    ) AS vehicle_no,


    -- 17-character chassis number
    CONCAT(
        'MA3EWDE1S',
        LPAD(n, 8, '0')
    ) AS chassis_no,


    -- Engine number
    CONCAT(
        'ENG',
        LPAD(n, 10, '0')
    ) AS engine_no,


    -- Vehicle model
    ELT(
        1 + MOD(n - 1, 6),

        'Maruti Swift VXI',
        'Hyundai Creta',
        'Tata Nexon',
        'Mahindra XUV300',
        'Honda City',
        'Toyota Glanza'

    ) AS model,


    -- Customer name
    CONCAT(

        ELT(
            1 + MOD(n - 1, 8),

            'Rahul',
            'Amit',
            'Priya',
            'Sneha',
            'Arjun',
            'Kavya',
            'Ravi',
            'Ananya'

        ),

        ' ',

        ELT(
            1 + MOD(n - 1, 8),

            'Sharma',
            'Patel',
            'Reddy',
            'Kumar',
            'Singh',
            'Nair',
            'Rao',
            'Das'

        )

    ) AS customer_name,


    -- 10-digit Indian-style mobile number
    CONCAT(
        '9',
        LPAD(
            MOD(
                987654321 + n,
                1000000000
            ),
            9,
            '0'
        )
    ) AS customer_mobile,


    -- Overdue amount between approximately ₹5,000 and ₹200,000
    ROUND(
        5000 + MOD(n * 7919, 195001),
        2
    ) AS overdue_amount


FROM numbers;