﻿
var dataTable;
$(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");

    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");

        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");

            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");

                }
                else {
                    loadDataTable("all");

                }
            }
        }
    }
}); 
function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + status },
        "pageLength": 5,
        "columns": [
            { data: 'id', "width": "3%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "3%" },
            { data: 'orderTotal', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class = "w-75 btn-group" role="group"> 
                        <a href="/Admin/Order/details?orderId=${data}" class="btn btn-primary "> <i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                "width": "15%" 
            }
            
        ]
    });
};
