colorSelected = '#FF0000';
colorNotEmpty = '#DA9521';
colorEmpty = '#aaa';
var mapGeoObjects;
var mapPOIs;
var poiObjectsToGeoObjects = {};
var contextMenuNewHandler = function(event){};
geoObjects = {};

optionsSelected = {
    color:colorSelected,
    weight:2
};


optionsEmpty = {
    color:colorEmpty,
    weight:2
};


optionsNotEmpty = {
    color:colorNotEmpty,
    weight:2
};


function setHash(hash) {
    try {
        if (window != window.top) {
            window.top.location.hash = hash;
        } else {
            location.hash = hash;
        }
    } catch (e) {}
}

function showMe()
{
	if (currentUser) {
		showBallonForPOIObject(currentUser);
	}
}

function getMapObjects() {
	$.ajax({
		url: serviceHost + "api/object",
		dataType: "json"
	}).done(function (data) {
		objectsArr = data;
		mapGeoObjects = {
			"type": "FeatureCollection",
			"features": []
		};
		for (var i = 0; i < objectsArr.length; i++) {
			mapGeoObjects.features.push({
				"type": "Feature",
				"geometry": {
					"type": objectsArr[i].Type,
					"coordinates": JSON.parse(objectsArr[i].Coords)
				},
				"properties": {
					"geoObjectId": objectsArr[i].Id
				}
			});
		}
		initMapData();
	}).fail(function () {
		alert("error getting objects");
	});
}

function getMapPois() {
	$.ajax({
		url: serviceHost + "api/poi",
		dataType: "json"
	}).done(function (data) {
		objectsArr = data;
		mapPOIs = {
			persons: {
				name: 'Коллеги',
				objects: []
			},
			objects: {
				name: 'Прочее',
				objects: []
			},
			confRooms: {
				name: 'Переговорки',
				objects: []
			},
            writeWalls: {
                name: 'Стены для рисования',
                objects: []
            }
		};

		for (var i = 0; i < objectsArr.length; i++) {
			if (objectsArr[i].BindedObjectIds.length > 0) {
				for (var j = 0; j < objectsArr[i].BindedObjectIds.length; j++)
					poiObjectsToGeoObjects[objectsArr[i].Id] = objectsArr[i].BindedObjectIds[j];
			}
			switch (objectsArr[i].Type) {
				case 0:
					mapPOIs.persons.objects.push({ "id": objectsArr[i].Id, "name": objectsArr[i].Name, "title": objectsArr[i].Title, "tel": objectsArr[i].Phone, "photoUrl": objectsArr[i].ImageUrl, "portalId": objectsArr[i].PortalId, "email": objectsArr[i].Email });
					break;
				case 1:
					mapPOIs.confRooms.objects.push({ "id": objectsArr[i].Id, "name": objectsArr[i].Name });
					break;
                case 2:
					mapPOIs.objects.objects.push({ "id": objectsArr[i].Id, "name": objectsArr[i].Name });
					break;
                case 3:
                    mapPOIs.writeWalls.objects.push({ "id": objectsArr[i].Id, "name": objectsArr[i].Name });
                    break;
			}
		}
		initMapData();
	}).fail(function () {
		alert("error getting objects");
	});
}

function initMapData() {
	if (mapGeoObjects == undefined || mapPOIs == undefined)
		return;

	initObjectsList();

	L.geoJson(mapGeoObjects, {
		style: function (feature) {
			var poiObject = getPoiObjectByGeoObjectId(feature.properties.geoObjectId);
			if (poiObject != null)
				return optionsNotEmpty;
			else
				return optionsEmpty;
		},
		onEachFeature: function (feature, layer) {
			var poiObject = getPoiObjectByGeoObjectId(feature.properties.geoObjectId);
			var poiObjectType = getPoiObjectTypeByGeoObjectId(feature.properties.geoObjectId);
			if (poiObject != undefined) {
				var popupText = renderPoiObjectToPopup(poiObject, poiObjectType);
				layer.bindPopup(popupText);
			} else {
				var popupEmptyText = "Объект №" + feature.properties.geoObjectId + "<br/>Продаем рекламу в popup'ах. Обращайтесь";
				layer.bindPopup(popupEmptyText);
			}
			layer.on('contextmenu', function (event) {
				contextMenuNewHandler(event);
			});
			layer.on('click', function (event) {
				if (poiObject != undefined) {
					setHash(poiObject.id);
				}
				event.target.openPopup();
				makeGeoObjectSelected(event.target);
			});
			geoObjects[feature.properties.geoObjectId] = layer;
		}
	}).addTo(myMap);

    try {
        var selectedPoiId = (window != window.top)
            ? (window.top.location.hash != "" ? window.top.location.hash.replace('#', '') : null)
            : (location.hash != "" ? location.hash.replace('#', '') : null);
        if (selectedPoiId.length > 0) {
            var poiObject = getPoiObjectByPoiId(selectedPoiId);
            if (poiObject != undefined)
                showBallonForPOIObject(poiObject.id);
        }
    }catch (e){}
}

function mapInit() {
    myMap = L.map('map', {
        crs:L.CRS.Simple
    });
    L.tileLayer('/Content/tiles-2013-02-13/{z}/tile-{x}-{y}.png', {
        noWrap:true,
        maxZoom:4,
        minZoom:2,
        attribution:"Идея и первоначальная реализация — mozornin, полная переделка и приведение в порядок — ekonovalov"
    }).addTo(myMap);
    
    myMap.setView([0.5, 0.5], 3);

    getMapObjects();
    getMapPois();
}


function initObjectsList() {
    for (poiObjectType in mapPOIs) {
        $("#search-select").append('<optgroup id="' + poiObjectType + '" label="' + mapPOIs[poiObjectType].name +'"></optgroup>');
        var selector = '#search-select optgroup[id="' + poiObjectType + '"]';
        for (poiObjectKey in mapPOIs[poiObjectType].objects) {
            var poiObject = mapPOIs[poiObjectType].objects[poiObjectKey];
            $(selector).append('<option value="' + poiObject.id + '">' + renderPoiObjectToChoosen(poiObject, poiObjectType) + '</option>');
        }
    }
    $(".chzn-select").chosen();
    $(".chzn-select-deselect").chosen({ allow_single_deselect:true });
}


function makeGeoObjectSelected(geoObject) {
    // restore current to its previous options
    makeGeoObjectUnselected();
    // set current to selected options
    window.currentGeoObjectOptions = geoObject.options;
    window.currentGeoObjectStruct = {
        geoObject:geoObject,
        options:$.extend({}, geoObject.options)
    }
    window.currentGeoObject = geoObject;
    window.currentGeoObject.setStyle(optionsSelected);
}


function makeGeoObjectUnselected() {
    if (window.currentGeoObjectStruct) {
        window.currentGeoObjectStruct.geoObject.setStyle(window.currentGeoObjectStruct.options);
    }
}


function showBallonForPOIObject(poiId) {
    setHash(poiId);
    geoObject = getGeoObjectByPoiObjectId(poiId);
    if (geoObject != undefined && geoObject != null) {
        geoObject.openPopup();
        makeGeoObjectSelected(geoObject);
        myMap.fitBounds(geoObject.getBounds());
    } else {
        alert('Объект ' + poiId + ' не найден. Напишите нам об этом, мы исправим.');
    }
}


function init() {
    mapInit();       
}

function getPoiObjectByPoiId(poiId) {

    for (var poiObjectType in mapPOIs) {
        var poiObjects = mapPOIs[poiObjectType].objects;
        for (var poiObjectKey in poiObjects) {
            var poiObject = poiObjects[poiObjectKey]
            if (poiObject.id == poiId) {
                return poiObject;
            }
        }
    }
}

function getPoiObjectByGeoObjectId(geoObjectId) {
    for (var poiObjectId in poiObjectsToGeoObjects) {
        if (poiObjectsToGeoObjects[poiObjectId] == geoObjectId) {
            return getPoiObjectByPoiId(poiObjectId);
        }
    }
}

function getPoiObjectTypeByGeoObjectId(geoObjectId) {
    var poiObject = getPoiObjectByGeoObjectId(geoObjectId);
    for (var poiObjectType in mapPOIs) {
        var poiObjects = mapPOIs[poiObjectType].objects;
        for (var poiObjectKey in poiObjects) {
            if (poiObjects[poiObjectKey] == poiObject) {
                return poiObjectType;
            }
        }
    }
}


function getGeoObjectByGeoObjectId(geoObj) {
    return geoObjects[geoObj];
}

function getGeoObjectIdByPoiId(poiId) {
    if (poiObjectsToGeoObjects[poiId] != undefined) {
        return poiObjectsToGeoObjects[poiId]
    }
}

function getGeoObjectByPoiObjectId(poiId) {
    return getGeoObjectByGeoObjectId(getGeoObjectIdByPoiId(poiId));
}

function getGeoIdByPoiId(poiId) {
    return poiObjectsToGeoObjects[poiId];
}


function renderPoiObjectToPopup(poiObject, poiObjectType) {
    var popupText = poiObject.name;;
    switch (poiObjectType) {
        case 'persons':
            popupText = "<div class='personInfo'><img class='userPic' src='" + poiObject.photoUrl + "' width ='42' align='left' class='personPhoto'/><span class='userName'><a target='_blank' href='http://portal.ptsecurity.ru/company/personal.php?page=user&user_id=" + poiObject.portalId +
                "'>" + poiObject.name + "</a></span><br/><span class='userTitle'>" + poiObject.title + "</span><br/>" +
                "<a class='email' href='mailto:" + poiObject.email + "'>" + poiObject.email + "</a>";
            if (poiObject.tel != '') {
                popupText += "<br/><span class='tel'>tel." + poiObject.tel + '</span>';
            }
            popupText += "</div>";
            break;

        case 'objects':
            popupText = poiObject.name;
            break;
    }
    return popupText;
}

function renderPoiObjectToChoosen(poiObject, poiObjectType) {
    var choosenText = poiObject.name;;
    switch (poiObjectType) {
        case 'persons':
            choosenText = poiObject.name + ', ' + poiObject.title
            break;

        case 'objects':
            choosenText = poiObject.name;
            break;
    }
    return choosenText;
}