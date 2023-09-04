export const environment = {
  customerCode: '',
  gMapKey: '',
  production: false,
  opapiUrl: '',
  logoPath: '',
  eyeShow: '',
  eyeHide: '',
  formatDate: '',
  viewDate: '',
  formatDateTime: '',
};

const hostName = location.hostname.split('.')[0];
const DomainCustomer = hostName.toString();


switch (DomainCustomer) {
  case 'op-dsc':
    environment.customerCode = 'dsc';
    environment.production = false;
    environment.gMapKey = 'AIzaSyBu4Y90CByGCKL5rxjR1bfCPAKVkh5KhYA';
    environment.opapiUrl = 'http://opdevapi.dsc.vn/api';
    environment.logoPath = '';
    environment.eyeShow = 'assets/layout/images/icon-pasword/eye-icon.svg';
    environment.eyeHide = 'assets/layout/images/icon-pasword/eye-hidden-icon.svg';
    environment.formatDate = 'DD/MM/YYYY';
    environment.viewDate = 'DD/MM/YYYY';
    environment.formatDateTime = 'yyyy/MM/dd';
    break;
  default:
    environment.customerCode = null;
    environment.production = false;
    environment.gMapKey = 'AIzaSyBu4Y90CByGCKL5rxjR1bfCPAKVkh5KhYA';
    environment.opapiUrl = 'http://opdevapi.dsc.vn/api',
    environment.logoPath = '';
    environment.eyeShow = 'assets/layout/images/icon-pasword/eye-icon.svg';
    environment.eyeHide = 'assets/layout/images/icon-pasword/eye-hidden-icon.svg';
    environment.formatDate = 'DD/MM/YYYY';
    environment.viewDate = 'DD/MM/YYYY';
    environment.formatDateTime = 'yyyy/MM/dd';
    break;
}
