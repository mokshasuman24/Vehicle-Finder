package com.example.vehiclefinder.data.repository

import com.example.vehiclefinder.data.api.RetrofitInstance
import com.example.vehiclefinder.data.model.SearchResponse

class VehicleRepository {

    suspend fun searchVehicle(last4: String): SearchResponse {
        return RetrofitInstance.api.searchVehicle(last4)
    }

    suspend fun searchChassis(last5: String): SearchResponse {
        return RetrofitInstance.api.searchChassis(last5)
    }
}