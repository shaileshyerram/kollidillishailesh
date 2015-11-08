
$(document).ready(function () {
    $(".datepicker").datepicker();
    $("#reset").on('click', function () {
        $('#reportForm')[0].reset.click();
        $(".reportData").empty();
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

                    $.each(data.Columns, function (i, cell) {
                        thead.append('<th class="columnCell">' + cell + '</th>');
                    });
                    tab.append(thead);
                     $.each(data.Rows, function (j, row) {
                         var trow = $('<tr></tr>');
                         $.each(row.RowCells, function (k, cell) {
                             trow.append('<td class="rowCell">' + cell + '</td>');
                         });
                         tab.append(trow);
                     });
                     $("tr:odd", tab).css('background-color', '#EAE9E9');
                     $(".reportData").empty().prepend(tab);
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