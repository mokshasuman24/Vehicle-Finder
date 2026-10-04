package com.example.vehiclefinder.ui.search

import androidx.compose.runtime.collectAsState
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedButton
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp

@Composable
fun SearchScreen(
    viewModel: SearchViewModel
) {

    var searchType by remember {
        mutableStateOf("Vehicle")
    }

    var query by remember {
        mutableStateOf("")
    }

    val uiState by viewModel.uiState.collectAsState()

    fun addCharacter(character: String) {

        val maxLength = if (searchType == "Vehicle") 4 else 5

        if (query.length < maxLength) {

            val newQuery = query + character
            query = newQuery

            // Automatic search
            if (newQuery.length == maxLength) {

                if (searchType == "Vehicle") {
                    viewModel.searchVehicle(newQuery)
                } else {
                    viewModel.searchChassis(newQuery)
                }
            }
        }
    }

    fun deleteCharacter() {
        if (query.isNotEmpty()) {
            query = query.dropLast(1)
            viewModel.resetState()
        }
    }

    fun clearQuery() {
        query = ""
        viewModel.resetState()
    }

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {

        Text(
            text = "Vehicle Finder",
            style = MaterialTheme.typography.headlineMedium
        )

        Spacer(modifier = Modifier.height(12.dp))

        Row(
            modifier = Modifier.fillMaxWidth(),
            horizontalArrangement = Arrangement.Center
        ) {

            if (searchType == "Vehicle") {
                Button(
                    onClick = {
                        searchType = "Vehicle"
                        query = ""
                        viewModel.resetState()
                    }
                ) {
                    Text("Vehicle No")
                }
            } else {
                OutlinedButton(
                    onClick = {
                        searchType = "Vehicle"
                        query = ""
                        viewModel.resetState()
                    }
                ) {
                    Text("Vehicle No")
                }
            }

            Spacer(modifier = Modifier.width(10.dp))

            if (searchType == "Chassis") {
                Button(
                    onClick = {
                        searchType = "Chassis"
                        query = ""
                        viewModel.resetState()
                    }
                ) {
                    Text("Chassis")
                }
            } else {
                OutlinedButton(
                    onClick = {
                        searchType = "Chassis"
                        query = ""
                        viewModel.resetState()
                    }
                ) {
                    Text("Chassis")
                }
            }
        }

        Spacer(modifier = Modifier.height(12.dp))

        Text(
            text = if (searchType == "Vehicle")
                "Enter last 4 digits"
            else
                "Enter last 5 characters"
        )

        Spacer(modifier = Modifier.height(8.dp))

        Text(
            text = if (query.isEmpty()) "----" else query,
            style = MaterialTheme.typography.headlineLarge
        )

        Spacer(modifier = Modifier.height(12.dp))

        // Keypad
        Row(
            horizontalArrangement = Arrangement.spacedBy(10.dp)
        ) {
            KeyButton("1") { addCharacter("1") }
            KeyButton("2") { addCharacter("2") }
            KeyButton("3") { addCharacter("3") }
        }

        Spacer(modifier = Modifier.height(8.dp))

        Row(
            horizontalArrangement = Arrangement.spacedBy(10.dp)
        ) {
            KeyButton("4") { addCharacter("4") }
            KeyButton("5") { addCharacter("5") }
            KeyButton("6") { addCharacter("6") }
        }

        Spacer(modifier = Modifier.height(8.dp))

        Row(
            horizontalArrangement = Arrangement.spacedBy(10.dp)
        ) {
            KeyButton("7") { addCharacter("7") }
            KeyButton("8") { addCharacter("8") }
            KeyButton("9") { addCharacter("9") }
        }

        Spacer(modifier = Modifier.height(8.dp))

        Row(
            horizontalArrangement = Arrangement.spacedBy(10.dp)
        ) {
            KeyButton("←") { deleteCharacter() }
            KeyButton("0") { addCharacter("0") }
            KeyButton("C") { clearQuery() }
        }

        Spacer(modifier = Modifier.height(16.dp))

        // Results
        when (val state = uiState) {

            is SearchUiState.Idle -> {
                Text("Enter a number to search")
            }

            is SearchUiState.Loading -> {
                CircularProgressIndicator()
                Spacer(modifier = Modifier.height(8.dp))
                Text("Searching...")
            }

            is SearchUiState.Error -> {
                Text(
                    text = "Error: ${state.message}",
                    color = MaterialTheme.colorScheme.error
                )
            }

            is SearchUiState.Success -> {

                if (state.response.results.isEmpty()) {

                    Text("No vehicles found")

                } else {

                    Text(
                        text = "${state.response.count} result(s) found",
                        style = MaterialTheme.typography.titleMedium
                    )

                    Spacer(modifier = Modifier.height(8.dp))

                    LazyColumn(
                        modifier = Modifier.fillMaxWidth()
                    ) {

                        items(state.response.results) { vehicle ->

                            Card(
                                modifier = Modifier
                                    .fillMaxWidth()
                                    .padding(vertical = 4.dp)
                            ) {

                                Column(
                                    modifier = Modifier.padding(12.dp)
                                ) {

                                    Text(
                                        text = vehicle.vehicleNo,
                                        style = MaterialTheme.typography.titleLarge
                                    )

                                    Text("Model: ${vehicle.model}")
                                    Text("Finance: ${vehicle.financeName}")
                                    Text("Branch: ${vehicle.branchName}")
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

@Composable
fun KeyButton(
    text: String,
    onClick: () -> Unit
) {

    Button(
        onClick = onClick,
        modifier = Modifier
            .width(80.dp)
            .height(55.dp)
    ) {
        Text(
            text = text,
            style = MaterialTheme.typography.titleLarge
        )
    }
}