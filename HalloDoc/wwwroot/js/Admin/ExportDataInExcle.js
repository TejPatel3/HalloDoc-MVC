﻿//for export all GamepadButton$('.exportall-download').click(function () {    console.log("Allexport")    $.ajax({        url: 'Admin/ExportAllDownload',        type: 'GET',        success: function (data) {            console.log(data);            var worksheet = XLSX.utils.json_to_sheet(data);            var workbook = XLSX.utils.book_new();            XLSX.utils.book_append_sheet(workbook, worksheet, 'Sheet1');            XLSX.writeFile(workbook, 'output.xlsx');        },        error: function (xhr, status, error) {            console.log(error);        }    })})//for export button$('button.download').on('click', function () {    console.log("excel-download")    var tabledata = $('#SixTableContent');    console.log("table" + tabledata)    var data = tableToJson(tabledata);    var wb = XLSX.utils.book_new();    var ws = XLSX.utils.json_to_sheet(data);    XLSX.utils.book_append_sheet(wb, ws, "Sheet1");    var wbout = XLSX.write(wb, { bookType: 'xlsx', type: 'binary' });    saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), 'data.xlsx');});function tableToJson(tabledata) {    var data = [];    var headers = [];    tabledata.find('th').each(function () {        headers.push($(this).text().trim());    });    console.log("headers : " + headers);    // Get the row data    tabledata.find('tr').each(function () {        var row = {}        $(this).find('td').each(function (i) {            row[headers[i]] = $(this).text().trim();        });        console.log(row)        data.push(row);    });    console.log("getdata" + data)    return data;}function s2ab(s) {    var buf = new ArrayBuffer(s.length);    var view = new Uint8Array(buf);    for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF;    return buf;}