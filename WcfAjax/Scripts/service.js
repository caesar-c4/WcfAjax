
function getAllHeroes()
{
    $.ajax(
        {
            url: "Services/Service.svc/GetAllHeroes",
            type: "GET",
            dataType: "json",
            success: function ( result )
            {
                heroes = result;
                drawHeroTable( result );
                console.log( result );
            }
        } );
}

function addHero()
{
    var newHero = {
        "FirstName": $( "#addFirstname" ).val(),
        "LastName": $( "#addLastname" ).val(),
        "HeroName": $( "#addHeroname" ).val(),
        "PlaceOfBirth": $( "#addPlaceOfBirth" ).val(),
        "Combat": $( "#addCombatPoints" ).val()
    };

    $.ajax(
        {
            url: "Services/Service.svc/AddHero",
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify( newHero ),
            success: function ()
            {
                showOverview();
            }
        } );
}
function putHero()
{
    updateHero.FirstName = $( "#updateFirstname" ).val();
    updateHero.LastName = $( "#updateLastname" ).val();
    updateHero.HeroName = $( "#updateHeroname" ).val();
    updateHero.PlaceOfBirth = $( "#updatePlaceOfBirth" ).val();
    updateHero.Combat = $( "#updateCombatPoints" ).val();

    $.ajax(
       {
           url: "Services/Service.svc/UpdateHero/" + updateHero.Id,
           type: "PUT",
           dataType: "json",
           contentType: "application/json",
           data: JSON.stringify( updateHero ),
           success: function ()
           {
               showOverview();
           }
       } );
}

function deleteHero( heroId )
{
    $.ajax(
      {
          url: "Services/Service.svc/DeleteHero/" + heroId,
          type: "DELETE",
          dataType: "json",
          success: function ( result )
          {
              getAllHeroes();
          }
      } );
}


function searchHero()
{
    var searchText = $( "#searchText" ).val();

    $.ajax(
       {
           url: "Services/Service.svc/SearchHero/" + searchText,
           type: "GET",
           dataType: "json",
           success: function ( result )
           {
               heroes = result;
               drawHeroTable( result );
           },
           error: function (error)
           {
               console.log( error.responseText );
               $( "#heroOverview" ).html( error.responseText );
           }
       } );
}

function sortedHeroList( type )
{
    $.ajax(
       {
           url: "Services/Service.svc/GetSortedHeroList/" + type,
           type: "GET",
           dataType: "json",
           success: function ( result )
           {
               drawHeroTable( result );
           }
       } );
}


function fight(  )
{
    var id1 = $( "#fighter1" ).val();
    var id2 = $( "#fighter2" ).val();
    $.ajax(
       {
           url: "Services/Service.svc/Fight/" + id1 + "/" + id2,
           type: "GET",
           dataType: "json",
           success: function ( result )
           {
               $( "#fightResult" ).html(result)
           }
       } );
}