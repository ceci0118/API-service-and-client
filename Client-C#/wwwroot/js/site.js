$(document).ready(function () {
    if ($('.delete').length > 1) {
        $('.delete').on('click', function (s) {
            var name = $(s.target).data("name");
            return confirm('Restaurant - ' + name + ' - will be deleted!');
        });
    }
    else {
        $('.delete').remove()
    }
});
