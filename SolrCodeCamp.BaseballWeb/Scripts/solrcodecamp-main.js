﻿/// <reference path="jquery-1.4.1-vsdoc.js" />
var currentSort = '';
var sortDir = 'A';

$(function () {
    $('.facet_box').live('change', updatePage);
    $('#btnSearch').live('click', updatePage);
    $('.result_sort').live('click', function (e) { setSort($(this)); });
    currentSort = $('.current_sort_header').attr('id').replace('id_', '');
    $('#txtSearch').keyup(function (e){
        if (e.keyCode == 13) {
            updatePage();
        }
    });
});

function setSort(arg) {
    var id = arg.attr('id').replace('id_', '');
    currentSort = id;
    var previousSort = $('.current_sort_header').attr('id').replace('id_', '');

    if (currentSort == previousSort) {
        if (sortDir == 'A') sortDir = 'D';
        else sortDir = 'A';
    }
    else {
        sortDir = 'A';
    }
    updatePage();
}

function updatePage() {
    var searchTerm = $('#txtSearch').val();
    $.blockUI({ css: { backgroundColor: '#f00', color: '#fff'} });
    var values = $('.facet_box').selectedValues();

    $.ajax({
        type: 'POST',
        url: '../Home/Query',
        success: success,
        data: 'searchTerm=' + searchTerm + '&sortTerm=' + currentSort + '&sortDir=' + sortDir + '&facets=' + values,
        dataType: 'html'
    });
}

function success(data) {
    $('#query_results').html(data);
    $.unblockUI();
}

