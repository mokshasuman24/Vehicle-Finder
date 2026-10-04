package com.example.vehiclefinder.ui.search

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.vehiclefinder.data.model.SearchResponse
import com.example.vehiclefinder.data.repository.VehicleRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

sealed class SearchUiState {
    data object Idle : SearchUiState()
    data object Loading : SearchUiState()
    data class Success(val response: SearchResponse) : SearchUiState()
    data class Error(val message: String) : SearchUiState()
}

class SearchViewModel : ViewModel() {

    private val repository = VehicleRepository()

    private val _uiState = MutableStateFlow<SearchUiState>(SearchUiState.Idle)
    val uiState: StateFlow<SearchUiState> = _uiState.asStateFlow()

    fun searchVehicle(last4: String) {

        if (last4.length != 4) return

        viewModelScope.launch {

            _uiState.value = SearchUiState.Loading

            try {

                val response = repository.searchVehicle(last4)

                _uiState.value = SearchUiState.Success(response)

            } catch (e: Exception) {

                _uiState.value = SearchUiState.Error(
                    e.message ?: "Something went wrong"
                )
            }
        }
    }

    fun searchChassis(last5: String) {

        if (last5.length != 5) return

        viewModelScope.launch {

            _uiState.value = SearchUiState.Loading

            try {

                val response = repository.searchChassis(last5)

                _uiState.value = SearchUiState.Success(response)

            } catch (e: Exception) {

                _uiState.value = SearchUiState.Error(
                    e.message ?: "Something went wrong"
                )
            }
        }
    }

    fun resetState() {
        _uiState.value = SearchUiState.Idle
    }
}