let infobox;

function GetMap() {
    const map = new Microsoft.Maps.Map('#map', {
        credentials: 'ApxKkcD7MByeyxSCRDPy27su9HDczhlYFmI96NQDgpTtdHXdNkLr2922DcqjyKad',
        center: new Microsoft.Maps.Location(32.091943, 34.820132), //SOKOLOV 14 EMPIRE
        mapTypeId: Microsoft.Maps.MapTypeId.road,
        zoom: 15
    });

    infobox = new Microsoft.Maps.Infobox(map.getCenter(), {
        visible: false
    });

    //Assign the infobox to a map instance.
    infobox.setMap(map);

    let randomLocations = Microsoft.Maps.TestDataGenerator.getLocations(5, map.getBounds());

    for (var i = 0; i < randomLocations.length; i++) {
        var pin = new Microsoft.Maps.Pushpin(randomLocations[i]);

        //Store some metadata with the pushpin.
        pin.metadata = {
            title: 'Pin ' + i,
            description: 'Discription for pin' + i
        };

        //Add a click event handler to the pushpin.
        Microsoft.Maps.Events.addHandler(pin, 'click', pushpinClicked);

        //Add pushpin to the map.
        map.entities.push(pin);
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