var $ = require("jquery");
let ajax = require("./AjaxCalls/Ajax.js")

$(document).ready(function(){
	ajax.totalLength()

	$("#form").submit(function(event) {
		event.preventDefault()
		ajax.GetSuggestions();
	});

	$("#newUser").submit(function(event) {
		event.preventDefault()
		ajax.newUser();
	});

	$("#newtrekk").submit(function(event) {
		event.preventDefault()
		ajax.RegisterTrekk()
	});

	$("#new").click(function(event) {
		$("#newUser").toggleClass("hide")
		$('.wrapper').toggleClass("blur")
	})

	$("#close").click(function(event) {
		$("#newUser").toggleClass("hide")
		$('.wrapper').toggleClass("blur")
	})

	$("#register").click(function(event) {
		$("#newtrekk").toggleClass("hide")
		$('.wrapper').toggleClass("blur")
	})

	$("#closeNewTrekk").click(function(event) {
		$("#newtrekk").toggleClass("hide")
		$('.wrapper').toggleClass("blur")
	})

	$('#filter').keyup(function() {
		ajax.Filter();
	})

	$('.filter').on('click','p',function(e) {
		$('input[name=sectionNumber]').val($(this).attr( "data-sectionValue" ))
		$('input[name=section]').val($(this).text())
	})

	$('#toplist').on('click', 'tr', async function(e) {
		$("#user").toggleClass("hide")
		$('.wrapper').toggleClass("blur")
		$('#username').html($(this).find('td:eq(1)').text())
		$('#userData').html('')
		 ajax.ParTrailProgression($(this).find('td:eq(1)').text())

	})

	$('#closeUser').click(function(e) {
		$("#user").toggleClass("hide")
		$('.wrapper').toggleClass("blur")
	})

		$('#closeSL').click(function(e) {
		$("#sl").toggleClass("hide")
		$('.wrapper').toggleClass("blur")
	})

	$('.trailsl').on('click','button',(function(e){
		$("#sl").toggleClass("hide")
		$('.wrapper').toggleClass("blur")
		$('#SLresult').html('')
		ajax.partTrail($(this).attr( "data-Value" ))
	}))
	
});

