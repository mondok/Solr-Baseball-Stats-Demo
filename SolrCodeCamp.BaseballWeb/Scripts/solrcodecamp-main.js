/// <reference path="jquery-1.4.1-vsdoc.js" />
var currentSort = '';
var sortDir = 'A';

$(function () {
    $('.facet_box').live('click', updatePage);
    $('.result_sort').live('click', function (e) { setSort($(this)); });
});

function selectFacetBox(arg){

}

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
    var values = $('.facet_box').selectedValues();

    $.ajax({
        type: 'POST',
        url: '../Home/Query',
        success: success,
        data: 'sortTerm=' + currentSort + '&sortDir=' + sortDir + '&facets=' + values,
        dataType: 'html'
    });
}

function success(data) {
    $('#query_results').html(data);
}

