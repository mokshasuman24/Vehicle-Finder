# Vehicle Finder

A vehicle and chassis search application built using:

- MySQL 8
- ASP.NET Core 8 Web API
- Kotlin
- Jetpack Compose
- Retrofit

## Features

- Search vehicle number using last 4 digits
- Search chassis using last 5 characters
- Indexed database search
- Automatic search after entering required characters
- Search results displayed as cards
- Loading and error states
- 50,000 seeded vehicle records

## Architecture

Android Jetpack Compose
→ ViewModel
→ Repository
→ Retrofit
→ ASP.NET Core Web API
→ MySQL

## Backend

Backend runs on:

http://localhost:5000

Android emulator uses:

http://10.0.2.2:5000/

## Database

Database contains:

- 5 finance companies
- 15 branches
- 50,000 vehicle records
- Search logs
- Indexed last 4 vehicle digits
- Indexed last 5 chassis characters

## API Endpoints

GET /api/health

GET /api/search/rc/{last4}

GET /api/search/chassis/{last5}

GET /api/records/{id}

GET /api/stats

## Performance

Vehicle and chassis suffixes are stored in indexed columns rather than using:

LIKE '%4821'

This allows MySQL to use the indexes for suffix searches.

## Test

Example vehicle search:

4821

Returns 5 matching records.