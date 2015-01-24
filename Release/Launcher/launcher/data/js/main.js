function pollMessages() {
	var messages = parity.pollMessages();
	$('#filler-text').html(messages[0]);
	$('#filler').css('width', messages[1] + '%');
	window.setTimeout(asd, 250);
};
function pollNews() {
	var messages = parity.pollNews();
	
};

$(document).ready(function () {
	"use strict";
	$('a.start').bind('click.start', function () {
		parity.startWarRock();
	});
	
	pollMessages();
	pollNews();
});