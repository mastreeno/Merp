(function ($) {
    var ChangeContactInfoViewModel = (function () {
        var ctor = function () {
            this.rootElement = $('#personContactInfoPane');
            this.originalId = this.rootElement.data('originalId');
            this.changeContactInfoUrl = this.rootElement.data('url');
            this.model = {};
            this._copyInitialValues();
        };

        ctor.prototype._copyInitialValues = function () {
            this.model = {
                phone: this.rootElement.find('.contact-phone').html().trim(),
                mobile: this.rootElement.find('.contact-mobile').html().trim(),
                fax: this.rootElement.find('.contact-fax').html().trim(),
                website: this.rootElement.find('.contact-website').html().trim(),
                email: this.rootElement.find('.contact-email').html().trim(),
                instantMessaging: this.rootElement.find('.contact-im').html().trim()
            };
        };

        ctor.prototype._replaceWithInputs = function () {
            var phoneInput = $('<input type="tel" class="form-control" />')
            phoneInput.val(this.model.phone);
            this.rootElement.find('.contact-phone').html(phoneInput);

            var mobileInput = $('<input type="text" class="form-control" />')
            mobileInput.val(this.model.mobile);
            this.rootElement.find('.contact-mobile').html(mobileInput);

            var faxInput = $('<input type="text" class="form-control" />')
            faxInput.val(this.model.fax);
            this.rootElement.find('.contact-fax').html(faxInput);

            var websiteInput = $('<input type="text" class="form-control" />')
            websiteInput.val(this.model.website);
            this.rootElement.find('.contact-website').html(websiteInput);

            var emailInput = $('<input type="email" class="form-control" />')
            emailInput.val(this.model.email);
            this.rootElement.find('.contact-email').html(emailInput);

            var instantMessagingInput = $('<input type="text" class="form-control" />')
            instantMessagingInput.val(this.model.instantMessaging);
            this.rootElement.find('.contact-im').html(instantMessagingInput);
        };

        ctor.prototype._replaceWithValues = function () {
            this.rootElement.find('.contact-phone').html(this.model.phone);
            this.rootElement.find('.contact-mobile').html(this.model.mobile);
            this.rootElement.find('.contact-fax').html(this.model.fax);
            this.rootElement.find('.contact-website').html(this.model.website);
            this.rootElement.find('.contact-email').html(this.model.email);
            this.rootElement.find('.contact-im').html(this.model.instantMessaging);
        };

        ctor.prototype.enableEdit = function () {
            this.rootElement.find('#changeContactInfoBtn').addClass('hidden');
            this._replaceWithInputs();
            this.rootElement.find('.change-contact-info-actions').removeClass('hidden');
        };

        ctor.prototype.cancel = function () {
            this.rootElement.find('.change-contact-info-actions').addClass('hidden');
            this._replaceWithValues();
            this.rootElement.find('#changeContactInfoBtn').removeClass('hidden');
        };

        ctor.prototype.save = function () {
            var values = {
                PersonId: this.originalId,
                PhoneNumber: this.rootElement.find('.contact-phone > input').val(),
                MobileNumber: this.rootElement.find('.contact-mobile > input').val(),
                FaxNumber: this.rootElement.find('.contact-fax > input').val(),
                WebsiteAddress: this.rootElement.find('.contact-website > input').val(),
                EmailAddress: this.rootElement.find('.contact-email > input').val(),
                InstantMessaging: this.rootElement.find('.contact-im > input').val()
            };

            var self = this;
            $.ajax(this.changeContactInfoUrl, {
                method: 'POST',
                data: values
            }).done(function (data, textStatus) {
                self.model.phone = values.PhoneNumber;
                self.model.mobile = values.MobileNumber;
                self.model.fax = values.FaxNumber;
                self.model.website = values.WebsiteAddress;
                self.model.email = values.EmailAddress;
                self.model.instantMessaging = values.InstantMessaging;

                self.cancel();
                alert('Contact info changed correctly');
            }).fail(function (xhr, textStatus, errorThrown) {
                alert('There was an error changing this info');
            });
        };

        return ctor;
    }());


    $(document).ready(function () {
        var changeContactInfo = new ChangeContactInfoViewModel();
        $('#changeContactInfoBtn').click(function (e) {
            changeContactInfo.enableEdit();
        });

        $('#cancelContactInfo').click(function (e) {
            changeContactInfo.cancel();
        });

        $('#saveContactInfo').click(function (e) {
            changeContactInfo.save();
        });
    });
}(jQuery));