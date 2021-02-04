// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function myFuction(index) {
    let input = $("#myInput").val();
    let table = $("#myTableBody");
    let tr = table.children("tr");
    for (let row of tr) {
        var td = row.children[index];
        console.log(input);
        if (input !== "") {
            console.log(input);
            if (td.textContent.toUpperCase().startsWith(input.toUpperCase())) {
                row.style.display = "";
            }
            else {
                row.style.display = "none";
            }
        }
        else {
            row.style.display = "";
        }
    }


};