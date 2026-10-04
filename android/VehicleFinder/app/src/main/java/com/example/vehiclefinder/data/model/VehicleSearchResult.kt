package com.example.vehiclefinder.data.model

import com.google.gson.annotations.SerializedName

data class VehicleSearchResult(
    val id: Int,

    @SerializedName("vehicleNo")
    val vehicleNo: String,

    @SerializedName("model")
    val model: String,

    @SerializedName("finance")
    val financeName: String?,

    @SerializedName("branch")
    val branchName: String?
)