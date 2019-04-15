var $ = require("jquery");

  /**
   * Get total length of Skåneled
   */
function totalLength () {
		$.get("/Home/TotalLength", (response) => {

			let deSerializedJson = parseJson(response)
			$('#totalLength').text(deSerializedJson.TotalLength)
			TopList()
			})
	
}

  /**
   * Get Section Sugestions from database
   */
function GetSuggestions() {
	 var formData = {
            'Distance': $('select[name=Distance]').val(),
            'Difficulty': $('select[name=Difficulty]').val(),
            'PartTrail': $('select[name=PartTrail]').val()
        };

	$.post("/Home/GetSuggestions", formData, function(response) {
		    $('select[name=Distance]').val('')
            $('select[name=Difficulty]').val('')
            $('select[name=PartTrail]').val('')

          let deSerializedJson = parseJson(response)
          let html = generateResultHtml(deSerializedJson)

         	$('#result').html(html)
	})
}

  /**
   * Parse Json to js
   */
	function parseJson(json) {
		 let obj = JSON.parse(json)
		 return obj
	}

  /**
   * Generate Html from Database result
   */
	function generateResultHtml(result)
	{

		let Html  = ""
		result.forEach(function(element) {
			let difficulty = ''
			switch(element.LevelOfDifficulty){
				case 1:
					difficulty = 'Green'
					break;
				case 2:
					difficulty = 'Red'
					break;
				case 3:
					difficulty = 'Black'
					break;
			}
			Html += `<div>
                <p>${element.Destination1} To ${element.Destination2}</p>
                <p>Length: ${element.Lenght/1000} Km</p>
                <p>Difficulty: ${difficulty}</p>
                <a href="${element.GPXLink}">GpxLink</a>
               <hr>
            </div>`
		})

		return Html
	}

  /**
   * Register New User
   */
	function newUser() {
			 var formData = {
            'name': $('input[name=name]').val(),
            'password': $('input[name=password]').val(),
            'email': $('input[name=email]').val()
        };
			$.post("/Home/NewUser", formData, function(response, status, xhr) {
			    $('.flash').html('New User Registered')
			    $('.flash').css( "background-color", "green");
			    $("#newUser").toggleClass("hide");
				$('.wrapper').toggleClass("blur")}
			).fail(function(data){
				$('.flash').html('Something went wrong, Try Again')
		    	$('.flash').css( "background-color", "red");
		    	$("#newUser").toggleClass("hide");
				('.wrapper').toggleClass("blur")
				})
	}

  /**
   * Register New trekk
   */
	function RegisterTrekk() {
			 var formData = {
            'name': $('input[name=Username]').val(),
            'password': $('input[name=Userpassword]').val(),
            'section': $('input[name=sectionNumber]').val()
        };
			$.post("/Home/NewTrekk", formData, function(response, status, xhr) {
			    $('.flash').html('New Trekk Registered')
			    $('.flash').css( "background-color", "green");
			    $("#newUser").toggleClass("hide");
				$('.wrapper').toggleClass("blur")
				TopList()
			}).fail(function(data){
				$('.flash').html('Something went wrong, Try Again')
		    	$('.flash').css( "background-color", "red");
		    	$("#newUser").toggleClass("hide");
				('.wrapper').toggleClass("blur")
				})
	}

  /**
   * Serach sectiosn based on keystroke
   */
	function Filter () {
		 var formData = {
            'partTrail': $('select[name=TrekkPartTrail]').val(),
            'filterString': $('input[name=section]').val()
        };

        if(formData.partTrail != null) {
        	$.post("/Home/searchSection", formData, function(response, status, xhr) {
			    let deSerializedJson = parseJson(response)

			    let html = ''


			    deSerializedJson.forEach(function(element) {
			    	html += `<p data-sectionValue=${element.id} class='filtervalue'>${element.Destination1} To ${element.Destination2}</p>`
			    })

			    $('.filter').html(html)
			})
        }
	}

	
	  /**
   * Generate toplist of trekkers
   */
	function TopList() {
		$.get("/Home/TopList", (response) => {
			let deSerializedJson = parseJson(response)
			let place = 0
			htmlToAdd = ""
			for (let key in deSerializedJson) {
				place++
				htmlToAdd += `
					<tr>
						<td>${place}</td>
					    <td class='trekker'>${key}</td>
					    <td>${deSerializedJson[key]}</td> 
					    <td>${((deSerializedJson[key]/$('#totalLength').text())*100).toFixed(2)} %</td>
					</tr>
				`
			}
			$("#toplist td").remove();

			$('#toplist tr:last').after(htmlToAdd);

		})
	}

  /**
   * Render History of trekked sections
   */
	function UserStatistic(user) {
			var formData = {
				'Username' : user
			}
				$.post("/Home/UserData", formData, function(response, status, xhr) {
			    
			    let deSerializedJson = parseJson(response)

			    deSerializedJson.forEach(item => {
			    	$(`#${item.PartOf}`).append(`<p>${item.Destination1} To ${item.Destination2}</p>`)
			    })


			}).fail(function(data){
				$('.flash').html('Something went wrong, Try Again')
		    	$('.flash').css( "background-color", "red");
		    	$("#newUser").toggleClass("hide");
		    	$("#user").toggleClass("hide");
				$('.wrapper').toggleClass("blur")
				})
	}

  /**
   * Render progression in length of each parttrail
   */
	function ParTrailProgression(user) {
			var formData = {
				'Username' : user
			}
			
			$.post("/Home/ParTrailProgression", formData, function(response, status, xhr) {
			    let deSerializedJson = parseJson(response)
			    deSerializedJson.forEach( item => {
					let html = `
					<div  id=${item.PartrailId}>
		            	
		            	<div class="header">
		                	<h3> ${item.Name} </h3>
		                	<p class="progress">Total Trekk : ${item.Length} Km </p>
		            	</div>
		            
		           
		        	</div>
        			`

					 $('#userData').append(html)
			    })

			    UserStatistic(user)

			}).fail(function(data){
				$('.flash').html('Something went wrong, Try Again')
		    	$('.flash').css( "background-color", "red");
		    	$("#newUser").toggleClass("hide");
		    	$("#user").toggleClass("hide");
				$('.wrapper').toggleClass("blur")
				})


	}

  /**
   * Render partrail and all its sections
   */
		function partTrail(id) {
			var formData = {
				'id' : id
			}
			$.post("/Home/partTrail", formData, function(response, status, xhr) {
			    let deSerializedJson = parseJson(response)
			    
			    deSerializedJson.forEach( item => {
					let difficulty = ""
						switch(item.LevelOfDifficulty){
							case 1:
								difficulty = 'Green'
								break;
							case 2:
								difficulty = 'Red'
								break;
							case 3:
								difficulty = 'Black'
								break;
						}
					let html = `<div class="border">
					<p>${item.Part}: ${item.Destination1} - ${item.Destination2}</p>
					<p>Length: ${item.Lenght/1000} Km</p>
					<p>Difficulty: ${difficulty}</p>
					<p>gpx: <a href=${item.GPXLink}>${item.GPXLink}</a></p>	    
					</div>`
					$("#SLresult").append(html)
			    })



			})

			
	}

module.exports = {
	GetSuggestions,
	newUser,
	RegisterTrekk,
	Filter,
	TopList,
	totalLength,
	UserStatistic,
	ParTrailProgression,
	partTrail

}