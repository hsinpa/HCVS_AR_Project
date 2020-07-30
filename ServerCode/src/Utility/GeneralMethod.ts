import {createHash} from 'crypto';

export function RollDice() {
    return Math.floor((Math.random() * 1.5) );
};

export function RandomRange(min : number, max : number) {
  return ~~(Math.random() * (max - min + 1)) + min
};

export function GenerateRandomString(p_len : number) {
  const shaString = createHash('sha1').update(new Date().toJSON()).digest('hex');

  if (p_len > shaString.length - 1)
    p_len = shaString.length - 1;

  return shaString.substring(0, p_len);
};