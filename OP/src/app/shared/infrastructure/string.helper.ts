export class StringHelper {
    static isNullOrEmpty(s: string): boolean {
        if (!s) {
            return true;
        } else {
            if (s.trim() === "") {
                return true;
            }
            return false;
        }
    }

    static isNumber(value: string | number): boolean {
        return ((value != null) && !isNaN(Number(value.toString())));
    }

    static isNumberAndNotContainDot(value: any): boolean {
        if (this.isNullOrEmpty(value)) {
            return false;
        }

        const indexDot = value.indexOf(".");
        if (indexDot > -1) {
            return false;
        }

        return ((value != null) && !isNaN(Number(value.toString())));
    }

    static isLengthAtLeast(value: any, minLength: number): boolean {
        const length = value.length;
        if (this.isNullOrEmpty(value)) {
            return false;
        }        
        else {
            if (length < minLength){
                return false;
            }
            else{
                return true;
            }
        }
    }
}