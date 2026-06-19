$(document).ready(function () {
	ShowHomeData();

});

function ShowHomeData() {
	var url = $('#urlHomeData').val();
	console.log("URL:", url);
	$.ajax({
		url: url,
		type: 'Get',
		dataType: 'json',
		contentType: 'application/json;charset=utf-8;',
		success: function (result, statu, xhr) {

			var object = '';

			$.each(result, function (index, item) {
				object += '<tr>';
				object += '<td>' + item.id + '</td>';    
				object += '<td>' + item.name + '</td>';  
				object += '<td>' + item.game + '</td>';  
				object += '<td>' + item.hour + '</td>';  
				object += '</tr>';
			});

			$('#table_data').html(object);
		},
		error: function (xhr, status, error) {
			console.log("Status:", xhr.status);
			console.log("Hata:", xhr.responseText);
			console.log("Status Text:", status);
			alert("Hata: " + xhr.status + " - " + error);
		}
	});
};

function ClearTextBox() {
	$('#Name').val('');
	$('#Game').val('');
	$('#Hour').val('');
}

function HideModalPopUp() {
	$('#HomeModal').modal('hide');
}
