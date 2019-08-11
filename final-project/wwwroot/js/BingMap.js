let infobox;

function GetMap() {
    const map = new Microsoft.Maps.Map('#map', {
        credentials: 'ApxKkcD7MByeyxSCRDPy27su9HDczhlYFmI96NQDgpTtdHXdNkLr2922DcqjyKad',
        center: new Microsoft.Maps.Location(32.07511, 34.80000), //SOKOLOV 14 EMPIRE
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 13
    });

    infobox = new Microsoft.Maps.Infobox(map.getCenter(), {
        visible: false
    });

    //Assign the infobox to a map instance.
    infobox.setMap(map);
    
    $.ajax({
        type: "GET",
        url: '/Branch/getBranches',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc,
        error: errorFunc
    })

    function errorFunc() {
        alert("is it too late now to say sorry?");
    }

    function successFunc(branches) {
        let randomLocations = Microsoft.Maps.TestDataGenerator.getLocations(5, map.getBounds());

        for (var key in branches) {
            let pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(branches[key].locationX, branches[key].locationY));

            //Store some metadata with the pushpin.
            pin.metadata = {
                title: branches[key].branchName,
                description: branches[key].addressInfo
            };

            //Add a click event handler to the pushpin.
            Microsoft.Maps.Events.addHandler(pin, 'click', pushpinClicked);

            //Add pushpin to the map.
            map.entities.push(pin);
        }
    }
}

function pushpinClicked(e) {
    //Make sure the infobox has metadata to display.
    if (e.target.metadata) {
        //Set the infobox options with the metadata of the pushpin.
        infobox.setOptions({
            location: e.target.getLocation(),
            title: e.target.metadata.title,
            description: e.target.metadata.description,
            visible: true
        });
    }
}