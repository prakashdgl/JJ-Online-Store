﻿function showDeleteWarning(categoryId) {
    $("#itemForDeleteValue").val(categoryId);
    $.fancybox("#deleteWarning");
}

(function () {
    $("#deleteBtn").click(() => {
        const $itemForDeleteValue = $("#itemForDeleteValue").val(); //TODO:encode data
        const deleteUrl = deleteCategoryUrl + `?categoryId=${$itemForDeleteValue}`;
        const request = {
            method: "DELETE",
            url: deleteUrl
        };
        $.fancybox.close();
        $.ajax(request)
            .then(() => {
                location.reload(true);
            });
    });
    $("#cancelBtn").click(() => {
        $.fancybox.close();
    });
})(deleteCategoryUrl);