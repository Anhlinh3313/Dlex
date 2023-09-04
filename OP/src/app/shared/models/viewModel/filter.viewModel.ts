export interface FilterViewModel<T = any> {
    pageSize?: number;
    pageNumber?: number;
    searchText?: string;
    countryId?: number;
    companyId?: number;
    districtid?: number;
    provinceId?: number;
    wardId?: number;
    isRemote?: boolean;
    userId?: number;
    cols?: any;
    centerHubId?: number;
    pOHubId?: number;
    dateFrom?: any;
    dateTo?: any;
    promotionTypeId?: number;
    isPublic?: boolean;
    isHidden?: boolean;
    isAccept?: boolean;
    customerId?: number;
    promotionId?: number;
    serviceId?: number;
    priceListId?: number;
    provinceFromId?: number;
    provinceToId?: number;
}
