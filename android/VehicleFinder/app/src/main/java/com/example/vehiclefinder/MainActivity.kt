package com.example.vehiclefinder

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import com.example.vehiclefinder.ui.search.SearchScreen
import com.example.vehiclefinder.ui.search.SearchViewModel
import com.example.vehiclefinder.ui.theme.VehicleFinderTheme

class MainActivity : ComponentActivity() {

    private val searchViewModel: SearchViewModel by viewModels()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            VehicleFinderTheme {
                SearchScreen(
                    viewModel = searchViewModel
                )
            }
        }
    }
}