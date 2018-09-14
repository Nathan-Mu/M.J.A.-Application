$("#checkall").click(function () {
    var ischecked = this.checked;
    $("input:checkbox[name='emails']").each(function () {
        this.checked = ischecked;
    });
});