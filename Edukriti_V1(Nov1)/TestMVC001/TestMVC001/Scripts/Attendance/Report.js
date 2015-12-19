
$(document).ready(function () {
    var groupManagerContainer = $(".treeContainer");
    $(groupManagerContainer).fancytree({
        icons: false,
        activate: function (event, data) {
            //Close the dropdown
            if ($.inArray(data.node.title, self.defaults.selectedGroups) === -1 && !data.node.selected) {
                self._addPills(data.node);
            }
        },

        beforeSelect: function (event, data) {
            var s;
        },

        expand: function (event, data) {
            event.stopPropagation();
            return false;
        },
        debugLevel: 0
    });
    var d = new Date();
    $(".btnExport").click(function () {
        $(".reportDataTable").table2excel({
            exclude: ".noExl",
            name: "AttendanceReport_" + d
        });
    });
    $(".lookUp .student.expandCollapse").click(function () {
        $(".lookUp .staff.panel-collapse").collapse('hide');
        $(".lookUp .student.panel-collapse").collapse('toggle');
        //$(".expandCollapse").toggleClass('collapsed');
    });
    $(".lookUp .staff.expandCollapse").click(function () {
        $(".lookUp .student.panel-collapse").collapse('hide');
        $(".lookUp .staff.panel-collapse").collapse('toggle');
        //$(".expandCollapse").toggleClass('collapsed');
    });


    //$(".form-control #DOBDatePicker").datepicker({
    //    changeMonth: true,//this option for allowing user to select month
    //    changeYear: true, //this option for allowing user to select from year range
    //    dateFormat: "dd-M-yy",
    //    setDate: new Date(),
    //    defaultDate: new Date(),
    //    yearRange: "-100:+0"
    //});

    $("#DateOfBirth").datepicker({
        changeMonth: true,//this option for allowing user to select month
        changeYear: true, //this option for allowing user to select from year range
        dateFormat: "dd-M-yy",
        setDate: new Date(),
        defaultDate: new Date(),
        yearRange: "-70:+0"
    });

    $("#fromDatepicker").datepicker({
        changeMonth: true,//this option for allowing user to select month
        changeYear: true, //this option for allowing user to select from year range
        dateFormat: "dd-M-yy",
        setDate: new Date(),
        defaultDate: new Date(),
        yearRange: "-30:+0"
    }).val(getTodaysDate(0));


    $("#toDatepicker").datepicker({
        changeMonth: true,//this option for allowing user to select month
        changeYear: true, //this option for allowing user to select from year range
        dateFormat: "dd-M-yy",
        yearRange: "-30:+0"
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
    $(".report #submit").on('click', function () {
        var reportRequestModel =
            {
                name: $("input[name='Name']").val(),
                studentclass: $('#Class').val(),
                section: $("input[name='Section']").val(),
                dtfrom: $("input[name='dateFrom']").val(),
                dtto: $("input[name='dateTo']").val(),
                category: $('#Category').val()
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
                        tcrow.append('<th class="columnCell ' + data.Columns[i] + '">' + cell + '</th>');
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
                    $("#error").empty();
                    $(".btnExport").removeClass("hide").addClass("show");
                } else {
                    $(".reportData").empty();
                    $("#error").empty();
                    $("<div class='text-danger alignCenter' id='error'>No records found. Please update the search criteria.</div>").insertAfter("#reportForm #buttons");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //Article.progress().hideLoading();
                $(".reportData").empty();
                $("<div class='text-danger alignCenter' id='error'>" +
                    "<p>status code: " + jqXHR.status + "</p>" +
                    "<p>errorThrown: " + errorThrown + "</p>" +
                    "<p>jqXHR.responseText: " + jqXHR.responseText + "</p></div>").insertAfter("#reportForm #buttons");
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
            success: function (message) {
                if (message.indexOf("Fail") > -1) {
                    $(".notificationResponse").empty().prepend("<div class='text-danger'>" + message + "</div>");
                } else {
                    $(".notificationResponse").empty().prepend("<div class='text-success bold'>Message sent successfully.</div>");
                    //Reset the form
                    $('#notificationForm')[0].reset.click();
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                //Article.progress().hideLoading();
                $(".notificationResponse").empty().prepend("<div class='text-danger'>" + textStatus + "</div>");
            }
        });
        return false;
    });


    // added for new notification form
    $("#message").keyup(function () {

        var maxLength = 120;
        var length = $(this).val().length;

        $("#message-characters").text(maxLength - length + " characters left.");

        if (length > maxLength) {

            $("#message-characters").css({
                "color": "#ccc"
            });

            $("#button-send").attr({
                "disabled": "disabled"
            });

        } else {

            $("#message-characters").css({
                "color": "#fff"
            });

            $("#button-send").removeAttr("disabled");
        }
    });



});




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

    return (day + '-' + months[month - 1] + '-' + year);
}





