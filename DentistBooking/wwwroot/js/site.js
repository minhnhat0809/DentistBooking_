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
connection.on("ReloadDentistSlots", function () {
    console.log("Reloading Dentist Slots")
    location.reload();
})
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
connection.on("ReloadPrescriptionMedicines", function () {
    console.log("Reloading PrescriptionMedicines")
    location.reload();
})
connection.on("ReloadServices", function () {
    console.log("Reloading Servives")
    location.reload();
})
connection.on("ReloadUsers", function () {
    console.log("Reloading Users")
    location.reload();
})
connection.on("ReloadClinics", function () {
    console.log("Reloading Clinic")
    location.reload();
})
connection.start().then(function () {
    console.log("Connected");
}).catch(function (err) {
    return console.error(err.toString());
});