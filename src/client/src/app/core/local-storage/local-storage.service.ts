import { Injectable } from '@angular/core';

class LocalStorage implements Storage {
    [name: string]: any;
    readonly length!: number;
    clear(): void { }
    getItem(key: string): string | null { return null; }
    key(index: number): string | null { return null; }
    removeItem(key: string): void { }
    setItem(key: string, value: string): void { }
}

@Injectable({ providedIn: 'root' })
export class LocalStorageService implements Storage {
    private storage: Storage;

    constructor() {
        this.storage = new LocalStorage();
    }

    [name: string]: any;

    public length!: number;

    public clear(): void {
        this.storage.clear();
    }

    public getItem(key: string): string | null {
        return this.storage.getItem(key);
    }

    public key(index: number): string | null {
        return this.storage.key(index);
    }

    public removeItem(key: string): void {
        return this.storage.removeItem(key);
    }

    public setItem(key: string, value: string): void {
        return this.storage.setItem(key, value);
    }
}
