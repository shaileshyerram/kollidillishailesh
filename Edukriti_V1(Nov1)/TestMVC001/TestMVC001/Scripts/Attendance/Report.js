
$(document).ready(function () {
    var d = new Date();
    $(".btnExport").click(function () {
        $(".reportDataTable").table2excel({
            exclude: ".noExl",
            name: "AttendanceReport_"+d
        });
    });

    //$("#fromDatepicker").datepicker({
    //    changeMonth: true,//this option for allowing user to select month
    //    changeYear: true, //this option for allowing user to select from year range
    //    dateFormat: "dd-M-yy",
    //    setDate: new Date(),
    //    defaultDate: new Date()
    //});

    $("#fromDatepicker").datepicker({
        changeMonth: true,//this option for allowing user to select month
        changeYear: true, //this option for allowing user to select from year range
        dateFormat: "dd-M-yy",
        setDate: new Date(),
        defaultDate: new Date()
    }).val(getTodaysDate(0));


    $("#toDatepicker").datepicker({
        changeMonth: true,//this option for allowing user to select month
        changeYear: true, //this option for allowing user to select from year range
        dateFormat: "dd-M-yy"
    }).val(getTodaysDate(0));


    $("#reset").on('click', function () {
        if ($('#reportForm').length > 0) {
            $('#reportForm')[0].reset.click();
            $(".reportData").empty();
        }
        if ($('#notificationForm').length > 0) {
            $('#notificationForm')[0].reset.click();
        }
    });
    $("#submit").on('click', function () {
        var reportRequestModel = 
            {
                name : $("input[name='name']").val(),
                studentclass : $("input[name='class']").val(),
                section: $("input[name='section']").val(),
                dtfrom: $("input[name='dateFrom']").val(),
                dtto: $("input[name='dateTo']").val()
            };
        //Article.progress().showLoading();
        $.ajax({
            url: $(this).attr("data-url"),
            type: "GET",
            //contentType: 'application/json; charset=UTF-8',
            data: reportRequestModel,
            datatype: "json",
            success: function (data) {
                if (data && data.Rows && data.Rows.length > 0) {
                    var tab = $('<table class="reportDataTable" rules="all"></table>');
                    
                    var thead = $('<thead></thead>');
                    var tcrow = $('<tr></tr>');
                    $.each(data.Columns, function (i, cell) {
                        tcrow.append('<th class="columnCell">' + cell + '</th>');
                    });
                    thead.append(tcrow);
                    tab.append(thead);
                     $.each(data.Rows, function (j, row) {
                         var trow = $('<tr></tr>');
                         $.each(row.RowCells, function (k, cell) {
                            trow.append('<td class="rowCell ' + data.Columns[k] + '">' + cell + '</td>');
                         });
                         tab.append(trow);
                     });
                     $("tr:odd", tab).css('background-color', '#EAE9E9');
                     $(".reportData").empty().prepend(tab);
                    $(".btnExport").show();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //Article.progress().hideLoading();
                alert("Error: " + error);
            }
        });
        return false;
    });

    $(".notificationContent #submit").on('click', function () {
        var reportRequestModel =
            {
                tophonenumber: $("textarea[name='phoneNumber']").val(),
                message: $("textarea[name='message']").val()
            };
        //Article.progress().showLoading();
        $.ajax({
            url: $(this).attr("data-url"),
            type: "GET",
            //contentType: 'application/json; charset=UTF-8',
            data: reportRequestModel,
            datatype: "json",
            success: function (data) {
                if (data && data.Rows && data.Rows.length > 0) {
                    var tab = $('<table class="reportDataTable" rules="all"></table>');

                    var thead = $('<thead></thead>');
                    var tcrow = $('<tr></tr>');
                    $.each(data.Columns, function (i, cell) {
                        tcrow.append('<th class="columnCell">' + cell + '</th>');
                    });
                    thead.append(tcrow);
                    tab.append(thead);
                    $.each(data.Rows, function (j, row) {
                        var trow = $('<tr></tr>');
                        $.each(row.RowCells, function (k, cell) {
                            trow.append('<td class="rowCell ' + data.Columns[k] + '">' + cell + '</td>');
                        });
                        tab.append(trow);
                    });
                    $("tr:odd", tab).css('background-color', '#EAE9E9');
                    $(".reportData").empty().prepend(tab);
                    $(".btnExport").show();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //Article.progress().hideLoading();
                alert("Error: " + error);
            }
        });
        return false;
    });
});

//function getTodaysDate(val) {
//    var t = new Date, day, month, year = t.getFullYear();
//    if (t.getDate() < 10) {
//        day = "0" + t.getDate();
//    }
//    else {
//        day = t.getDate();
//    }
//    if ((t.getMonth() + 1) < 10) {
//        month = "0" + (t.getMonth() + 1 - val);
//    }
//    else {
//        month = t.getMonth() + 1 - val;
//    }

//    return (day + '/' + month + '/' + year);
//}




function getTodaysDate(val) {
    var t = new Date, day, month, year = t.getFullYear();
    //var months = ["Janurary", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    var months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    if (t.getDate() < 10) {
        day = "0" + t.getDate();
    }
    else {
        day = t.getDate();
    }
    if ((t.getMonth() + 1) < 10) {
        month = "0" + (t.getMonth() + 1 - val);
    }
    else {
        month = t.getMonth() + 1 - val;
    }

    return (day + '-' + months[month-1] + '-' + year);
}