import { BusinessCard } from '../services/business-card-service';

export function parseVCard(vCardString: string): Partial<BusinessCard> | null {
    if (!vCardString || !vCardString.includes('BEGIN:VCARD')) {
        return null;
    }

    const result: Partial<BusinessCard> = {};
    const lines = vCardString.split(/\r?\n/);

    for (const line of lines) {
        if (line.startsWith('FN:')) {
            result.name = line.substring(3).trim();
        } else if (line.startsWith('EMAIL') && line.includes(':')) {
            result.email = line.split(':').slice(1).join(':').trim();
        } else if (line.startsWith('TEL') && line.includes(':')) {
            result.phoneNumber = line.split(':').slice(1).join(':').trim();
        } else if (line.startsWith('ADR') && line.includes(':')) {
            const parts = line.split(':').slice(1).join(':').split(';');
            result.address = parts.filter(p => p.trim() !== '').join(', ').trim();
        } else if (line.startsWith('BDAY:')) {
            result.dob = line.substring(5).trim();
        } else if (line.startsWith('X-GENDER:')) {
            const genderStr = line.substring(9).trim().toLowerCase();
            if (genderStr === 'male' || genderStr === 'm') result.gender = 'Male';
            else if (genderStr === 'female' || genderStr === 'f') result.gender = 'Female';
        }
    }

    return result;
}
