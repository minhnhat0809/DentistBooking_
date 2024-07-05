// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let menuicn = document.querySelector(".menuicn");
let nav = document.querySelector(".navcontainer");

menuicn.addEventListener("click", () => {
    nav.classList.toggle("navclose");
})
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/SignalRHub").build();

connection.on("ReloadAppointments", function () {
    console.log("Reloading Appointments")
    location.reload();
})
connection.on("ReloadCheckupSchedules", function () {
    console.log("Reloading CheckupSchedules")
    location.reload();
})
connection.on("ReloadMedicalRecords", function () {
    console.log("Reloading MedicalRecords")
    location.reload();
})
connection.on("ReloadMedicines", function () {
    console.log("Reloading Medicines")
    location.reload();
})
connection.on("ReloadPrescriptions", function () {
    console.log("Reloading Prescriptions")
    location.reload();
})
connection.start().then(function () {
    console.log("Connected");
}).catch(function (err) {
    return console.error(err.toString());
});