$(document).ready(function () {
    ShowUserGamerData();
});

function ShowUserGamerData() {
    var url = $('#urlGamerListUser').val();
    $.ajax({
        url: url,
        type: 'Get',
        dataType: 'json',
        contentType: 'application/json;charset=utf-8;',
        success: function (result) {
            var object = '';
            $.each(result, function (index, item) {
                object += '<tr>';
                object += '<td>' + item.id + '</td>';
                object += '<td>' + item.name + '</td>';
                object += '<td>' + item.game + '</td>';
                object += '<td>' + item.hour + '</td>';
                object += '</tr>';
            });
            $('#user_table_data').html(object);
        },
        error: function () {
            alert("Veri yüklenemedi");
        }
    });
}

$('#btnUserAdd').click(function () {
    var objData = {
        Name: $('#userName').val(),
        Game: $('#userGame').val(),
        Hour: $('#userHour').val(),
    };

    if (!objData.Name || !objData.Game || !objData.Hour) {
        alert('Lütfen tüm alanları doldurun');
        return;
    }

    $.ajax({
        url: '/Gamer/AddGamer',
        type: 'Post',
        data: objData,
        contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
        dataType: 'json',
        success: function () {
            alert('Kayıt eklendi');
            $('#userName').val('');
            $('#userGame').val('');
            $('#userHour').val('');
            ShowUserGamerData();
        },
        error: function () {
            alert("Kayıt eklenemedi");
        }
    });
});