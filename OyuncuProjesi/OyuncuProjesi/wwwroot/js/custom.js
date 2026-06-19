$(document).ready(function () {
	ShowGamerData();

});

function ShowGamerData() {
	var url = $('#urlGamerData').val();
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
				object += '<td><a href="#" class="btn btn-primary" onclick="Edit(' + item.id + ')">Edit</a> <a href="#" class="btn btn-danger" onclick="Delete(' + item.id + ');">Delete</a></td>';
				object += '</tr>'


			});

			$('#table_data').html(object);
		},
		error: function () {
			alert("veri yüklenemdi");
		}
	});
};

$('#btnAddGamer').click(function () {
	ClearTextBox();
	$('#GamerMadal').modal('show');
	$('#GamerId').hide();
	$('#AddGamer').css('display', 'block');
	$('#btnUpdate').css('display', 'none');
	$('#gamerHeading').text('Add Gamer');
})


function AddGamer() {

	var objData = {
		Name: $('#Name').val(),
		Game: $('#Game').val(),
		Hour: $('#Hour').val(),

	}
	$.ajax({
		url: '/Gamer/AddGamer',
		type: 'Post',
		data: objData,
		contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
		dataType: 'json',
		success: function () {
			alert('Data Saved');
			ClearTextBox();
			ShowGamerData();
			HideModalPopUp();
		},


		error: function () {

			alert("veri yüklenemedi");
		}

	});
}

function ClearTextBox() {
	$('#Name').val('');
	$('#Game').val('');
	$('#Hour').val('');
}

function HideModalPopUp() {
	$('#GamerMadal').modal('hide');
}

function Delete(id) {
	if (confirm('Are you sure, You want to delete this record?')) {
		$.ajax({
			url: '/Gamer/Delete?id=' + id,
			success: function () {
				alert('Record Deleted!');
				ShowGamerData();
			},
			error: function () {
				alert("Data can't be deleted!");
			}
		})
	}
}


function Edit(id) {

	$.ajax({
		url: '/Gamer/Edit?id=' + id,
		type: 'Get',
		contentType: 'application/json;charset=utf-8',
		dataType: 'json',
		success: function (response) {
			$('#GamerMadal').modal('show');
			$('#GamerId').val(response.id);
			$('#Name').val(response.name);
			$('#Game').val(response.game);
			$('#Hour').val(response.hour);
			$('#AddGamer').css('display', 'none');
			$('#btnUpdate').css('display', 'block');
			$('#gamerHeading').text('Update Record');
		},
		error: function () {
			alert('Data not found');
		}
	})
}

function UpdateGamer() {
	var objData = {
		Id: $('#GamerId').val(),
		Name: $('#Name').val(),
		Game: $('#Game').val(),
		Hour: $('#Hour').val(),
	}
	$.ajax({
		url: '/Gamer/Update',
		type: 'Post',
		data: objData,
		contentType: 'application/x-www-form-urlencoded;charset=utf-8;',
		dataType: 'json',
		success: function () {
			alert('Data Updated');
			HideModalPopUp();
			ShowGamerData();
			ClearTextBox();
		},
		error: function () {
			alert("Data can't Saved!");
		}
	})
}