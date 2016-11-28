var AppCalendar = function() {

    return {
        //main function to initiate the module
        init: function() {
            this.initCalendar();
        },

        initCalendar: function() {

            if (!jQuery().fullCalendar) {
                return;
            }

            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();

            var h = {};

            if (App.isRTL()) {
                if ($('#calendar').parents(".portlet").width() <= 720) {
                    $('#calendar').addClass("mobile");
                    h = {
                        right: 'title, prev, next',
                        center: '',
                        left: 'agendaDay, agendaWeek, today'
                    };
                } else {
                    $('#calendar').removeClass("mobile");
                    h = {
                        right: 'title',
                        center: '',
                        left: 'agendaDay, agendaWeek, today, prev,next'
                    };
                }
            } else {
                if ($('#calendar').parents(".portlet").width() <= 720) {
                    $('#calendar').addClass("mobile");
                    h = {
                        left: 'title, prev, next',
                        center: '',
                        right: 'today,agendaWeek,agendaDay'
                    };
                } else {
                    $('#calendar').removeClass("mobile");
                    h = {
                        left: 'title',
                        center: '',
                        right: 'prev,next,today,agendaWeek,agendaDay'
                    };
                }
            }

            var initDrag = function(el) {
                // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
                // it doesn't need to have a start or end
                var eventObject = {
                    title: $.trim(el.text()) // use the element's text as the event title
                };
                // store the Event Object in the DOM element so we can get to it later
                el.data('eventObject', eventObject);
                // make the event draggable using jQuery UI
                el.draggable({
                    zIndex: 999,
                    revert: true, // will cause the event to go back to its
                    revertDuration: 0 //  original position after the drag
                });
            };

            var addEvent = function(title) {
                title = title.length === 0 ? "Untitled Event" : title;
                var html = $('<div class="external-event label label-default">' + title + '</div>');
                jQuery('#event_box').append(html);
                initDrag(html);
            };

            $('#external-events div.external-event').each(function() {
                initDrag($(this));
            });

            $('#event_add').unbind('click').click(function() {
                var title = $('#event_title').val();
                addEvent(title);
            });

            

            $('#calendar').fullCalendar('destroy'); // destroy the calendar
            $('#calendar').fullCalendar({ //re-initialize the calendar
                header: h,
                defaultView: 'agendaWeek', // change default view with available options from http://arshaw.com/fullcalendar/docs/views/Available_Views/ 
                slotMinutes: 15,
                editable: true,
                droppable: true, // this allows things to be dropped onto the calendar !!!
                drop: function(date, allDay) { // this function is called when something is dropped

                    // retrieve the dropped element's stored Event Object
                    var originalEventObject = $(this).data('eventObject');
                    // we need to copy it, so that multiple events don't have a reference to the same object
                    var copiedEventObject = $.extend({}, originalEventObject);

                    // assign it the date that was reported
                    copiedEventObject.start = date;
                    copiedEventObject.allDay = allDay;
                    copiedEventObject.className = $(this).attr("data-class");

                    // render the event on the calendar
                    // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
                    $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

                    // is the "remove after drop" checkbox checked?
                    if ($('#drop-remove').is(':checked')) {
                        // if so, remove the element from the "Draggable Events" list
                        $(this).remove();
                    }
                },
                events: [{
                    title: 'Adam - Server',
                    start: new Date(y, m, d+1, 11, 15),
                    end: new Date(y, m, d+1, 15, 45),
                    backgroundColor: App.getBrandColor('yellow'),
                    allDay: false,
                }, {
                    title: 'Andrew - Manager',
                    start: new Date(y, m, d+1, 9, 30),
                    end: new Date(y, m, d+1, 17, 30),
                    allDay: false,
                }, {
                    title: 'Ahmad - Cook',
                    start: new Date(y, m, d+1, 8, 00),
                    end: new Date(y, m, d+1, 13, 30),
                    backgroundColor: App.getBrandColor('green'),
                    allDay: false,
                }, {
                    title: 'Adam - Server',
                    start: new Date(y, m, d+2, 11, 15),
                    end: new Date(y, m, d+2, 15, 45),
                    backgroundColor: App.getBrandColor('yellow'),
                    allDay: false,
                }, {
                    title: 'Andrew - Manager',
                    start: new Date(y, m, d+2, 9, 30),
                    end: new Date(y, m, d+2, 17, 30),
                    allDay: false,
                }, {
                    title: 'Ahmad - Cook',
                    start: new Date(y, m, d+2, 8, 00),
                    end: new Date(y, m, d+2, 13, 30),
                    backgroundColor: App.getBrandColor('green'),
                    allDay: false,
                }, {
                    title: 'Adam - Server',
                    start: new Date(y, m, d+3, 11, 15),
                    end: new Date(y, m, d+3, 15, 45),
                    backgroundColor: App.getBrandColor('yellow'),
                    allDay: false,
                }, {
                    title: 'Andrew - Manager',
                    start: new Date(y, m, d+3, 9, 30),
                    end: new Date(y, m, d+3, 17, 30),
                    allDay: false,
                }, {
                    title: 'Ahmad - Cook',
                    start: new Date(y, m, d+3, 8, 00),
                    end: new Date(y, m, d+3, 13, 30),
                    backgroundColor: App.getBrandColor('green'),
                    allDay: false,
                }, {
                    title: 'Adam - Server',
                    start: new Date(y, m, d+4, 11, 15),
                    end: new Date(y, m, d+4, 15, 45),
                    backgroundColor: App.getBrandColor('yellow'),
                    allDay: false,
                }, {
                    title: 'Andrew - Manager',
                    start: new Date(y, m, d+4, 9, 30),
                    end: new Date(y, m, d+4, 17, 30),
                    allDay: false,
                }, {
                    title: 'Ahmad - Cook',
                    start: new Date(y, m, d+4, 8, 00),
                    end: new Date(y, m, d+4, 13, 30),
                    backgroundColor: App.getBrandColor('green'),
                    allDay: false,
                },
                {
                    title: 'Adam - Server',
                    start: new Date(y, m, d+5, 11, 15),
                    end: new Date(y, m, d+5, 15, 45),
                    backgroundColor: App.getBrandColor('yellow'),
                    allDay: false,
                }, {
                    title: 'Andrew - Manager',
                    start: new Date(y, m, d+5, 9, 30),
                    end: new Date(y, m, d+5, 17, 30),
                    allDay: false,
                }, {
                    title: 'Ahmad - Cook',
                    start: new Date(y, m, d+5, 8, 00),
                    end: new Date(y, m, d+5, 13, 30),
                    backgroundColor: App.getBrandColor('green'),
                    allDay: false,
                } ]
            });

        }

    };

}();

jQuery(document).ready(function() {    
   AppCalendar.init(); 
});