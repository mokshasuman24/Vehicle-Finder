package com.example.vehiclefinder.data.model

data class SearchResponse(
    val count: Int,
    val results: List<VehicleSearchResult>
)