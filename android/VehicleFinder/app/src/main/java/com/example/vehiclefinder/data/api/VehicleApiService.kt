package com.example.vehiclefinder.data.api

import com.example.vehiclefinder.data.model.SearchResponse
import retrofit2.http.GET
import retrofit2.http.Path

interface VehicleApiService {

    @GET("api/search/rc/{last4}")
    suspend fun searchVehicle(
        @Path("last4") last4: String
    ): SearchResponse

    @GET("api/search/chassis/{last5}")
    suspend fun searchChassis(
        @Path("last5") last5: String
    ): SearchResponse
}