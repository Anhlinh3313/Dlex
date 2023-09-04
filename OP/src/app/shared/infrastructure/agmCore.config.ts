import { environment } from 'src/environments/environment';

export const AgmCoreConfig = {
    apiKey: environment.gMapKey,
    libraries: ['geometry', 'drawing', 'places'],
    language: 'vi',
    region: 'VN'
};
