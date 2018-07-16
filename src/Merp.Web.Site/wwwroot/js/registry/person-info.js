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

            if (this.rootElement.find('.alert.alert-danger').length) {
                this.rootElement.find('.alert.alert-danger').remove();
            }
        };

        ctor.prototype.save = function () {
            this.rootElement.find('.change-contact-info-actions > button').prop('disabled', true);
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
                self.rootElement.find('.change-contact-info-actions > button').prop('disabled', false);
                alert('Contact info changed correctly');
            }).fail(function (xhr, textStatus, errorThrown) {
                self.rootElement.find('.change-contact-info-actions > button').prop('disabled', false);
                self._buildErrorList(xhr.responseJSON);
            });
        };

        ctor.prototype._buildErrorList = function (errors) {
            var alertContainer = $('<div class="alert alert-danger alert-dismissable" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div>');
            var errorList = $('<ul></ul>');
            for (var key in errors) {
                errorList.append($('<li>' + errors[key].join(',') + '</li>'));
            }
            alertContainer.append(errorList);

            this.rootElement.prepend(alertContainer);
        };

        return ctor;
    }());

    var ChangeAddressViewModel = (function () {
        var ctor = function () {
            this.rootElement = $('#personAddressSection');
            this.originalId = this.rootElement.data('originalId');
            this.changeAddressUrl = this.rootElement.attr('action');
            this.model = {};
            this._copyInitialValues();
        };

        ctor.prototype._copyInitialValues = function () {
            this.model = {
                address: this.rootElement.find('.address').html().trim(),
                postalCode: this.rootElement.find('.postal-code').html().trim(),
                city: this.rootElement.find('.city').html().trim(),
                country: this.rootElement.find('.country').html().trim(),
                province: this.rootElement.find('.province').html().trim(),
            };
        };

        ctor.prototype._replaceWithValues = function () {
            this.rootElement.find('.address').html(this.model.address);
            this.rootElement.find('.postal-code').html(this.model.postalCode);
            this.rootElement.find('.city').html(this.model.city);
            this.rootElement.find('.country').html(this.model.country);
            this.rootElement.find('.province').html(this.model.province);
            this.rootElement.find('.effective-date').remove();
        };

        ctor.prototype._replaceWithInputs = function () {
            var addressInput = $('<input type="text" class="form-control" id="Address_Address" name="Address.Address" data-val="true" data-val-required="The address is required" /><span class="text-danger field-validation-valid" data-valmsg-for="Address.Address" data-valmsg-replace="true"></span>');
            addressInput.val(this.model.address);
            this.rootElement.find('.address').html(addressInput);

            var postalCodeInput = $('<input type="text" class="form-control" id="Address_PostalCode" name="Address.PostalCode" />');
            postalCodeInput.val(this.model.postalCode);
            this.rootElement.find('.postal-code').html(postalCodeInput);

            var cityInput = $('<input type="text" class="form-control" id="Address_City" name="Address.City" data-val="true" data-val-required="The city is required" /><span class="text-danger field-validation-valid" data-valmsg-for="Address.City" data-valmsg-replace="true"></span>');
            cityInput.val(this.model.city);
            this.rootElement.find('.city').html(cityInput);

            var countryInput = $('<input type="text" class="form-control" id="Address_Country" name="Address.Country" />');
            countryInput.val(this.model.country);
            this.rootElement.find('.country').html(countryInput);

            var provinceInput = $('<input type="text" class="form-control" id="Address_Province" name="Address.Province" />');
            provinceInput.val(this.model.province);
            this.rootElement.find('.province').html(provinceInput);

            var effectiveDate = $('<dt class="effective-date">Effective date</dt><dd class="effective-date effective-date-input"><input type="date" class="form-control" name="EffectiveDate" id="EffectiveDate" /></dd>');
            this.rootElement.find('.province').after(effectiveDate);

            this.rootElement.removeData("validator").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(this.rootElement);
        };

        ctor.prototype.enableEdit = function () {
            this.rootElement.find('#changeAddressBtn').addClass('hidden');
            this._replaceWithInputs();
            this.rootElement.find('.change-address-actions').removeClass('hidden');
        };

        ctor.prototype.cancel = function () {
            this.rootElement.find('.change-address-actions').addClass('hidden');
            this._replaceWithValues();
            this.rootElement.find('#changeAddressBtn').removeClass('hidden');

            if (this.rootElement.find('.alert.alert-danger').length) {
                this.rootElement.find('.alert.alert-danger').remove();
            }
        };

        ctor.prototype.save = function () {
            if (!this.rootElement.valid()) {
                return;
            }

            this.rootElement.find('.change-address-actions > button').prop('disabled', true);
            var values = {
                PersonId: this.originalId,
                Address: {
                    Address: this.rootElement.find('.address > input').val(),
                    City: this.rootElement.find('.city > input').val(),
                    PostalCode: this.rootElement.find('.postal-code > input').val(),
                    Province: this.rootElement.find('.province > input').val(),
                    Country: this.rootElement.find('.country > input').val()
                },
                EffectiveDate: this.rootElement.find('.effective-date-input > input').val()
            };

            var self = this;
            $.ajax(this.changeAddressUrl, {
                method: 'POST',
                data: values
            }).done(function (data, textStatus) {
                self.model.address = values.Address.Address;
                self.model.city = values.Address.City;
                self.model.postalCode = values.Address.PostalCode;
                self.model.province = values.Address.Province;
                self.model.country = values.Address.Country;

                self.cancel();
                self.rootElement.find('.change-address-actions > button').prop('disabled', false);
                alert('Address changed correctly');
            }).fail(function (xhr, textStatus, errorThrown) {
                self.rootElement.find('.change-address-actions > button').prop('disabled', false);
                self._buildErrorList(xhr.responseJSON);
            });
        };

        ctor.prototype._buildErrorList = function (errors) {
            var alertContainer = $('<div class="alert alert-danger alert-dismissable" role="alert"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button></div>');
            var errorList = $('<ul></ul>');
            for (var key in errors) {
                errorList.append($('<li>' + errors[key].join(',') + '</li>'));
            }
            alertContainer.append(errorList);

            this.rootElement.prepend(alertContainer);
        };

        return ctor;
    }());

    $(document).ready(function () {
        var changeAddress = new ChangeAddressViewModel();
        var changeContactInfo = new ChangeContactInfoViewModel();
        $('#changeContactInfoBtn').click(function (e) {
            e.preventDefault();
            changeContactInfo.enableEdit();
        });

        $('#cancelContactInfo').click(function (e) {
            e.preventDefault();
            changeContactInfo.cancel();
        });

        $('#saveContactInfo').click(function (e) {
            e.preventDefault();
            changeContactInfo.save();
        });

        $('#changeAddressBtn').click(function (e) {
            e.preventDefault();
            changeAddress.enableEdit();
        });

        $('#cancelAddress').click(function (e) {
            e.preventDefault();
            changeAddress.cancel();
        });

        $('#personAddressSection').submit(function (e) {
            e.preventDefault();
            changeAddress.save();
        });
    });
}(jQuery));