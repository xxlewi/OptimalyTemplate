-- Initial database setup for {{APP_NAME}}
-- This script will be executed when the PostgreSQL container starts

-- Database is created automatically by POSTGRES_DB environment variable
-- This script can be used for additional initialization if needed

-- Create any additional extensions or setup here
-- For example:
-- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Database is now ready for Entity Framework migrations
\echo '{{APP_NAME}} database initialized successfully';