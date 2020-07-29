import {createHash} from 'crypto';

export function RollDice() {
    return Math.floor((Math.random() * 1.5) );
};

export function RandomRange(min : number, max : number) {
  return ~~(Math.random() * (max - min + 1)) + min
};

export function GenerateRandomString(p_extraString = "") {
  const shaString = createHash('sha1').update(new Date().toJSON() + p_extraString).digest('hex');
  return shaString;    
};