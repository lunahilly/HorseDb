function OpenDialog(horseName, id) {
	const dialog = document.getElementById(id);
	var dialogBody = document.getElementById("DialogText");
	dialogBody.innerHTML = "You are about to delete " + horseName + ", are you sure?";
	dialog.showModal();
}
function ShowErrorDialog() {
	const dialog = document.getElementById("ErrorDialog");
	var dialogBody = document.getElementById("ErrorText");
	dialogBody.innerHTML = "There are still horses attached to this owner, please delete those first.";
	dialog.showModal();
}
function CloseDialog(id) {
	const dialog = document.getElementById(id);
	dialog.close();
}
