export enum UserRoles {
  None,
  Owner,
  Admin,
  Resident,
}
export interface LoginModel {
  email: string;
  password: string;
}

export interface RegisterModel {
  username: string;
  password: string;
}

export interface JWTAuthToken {
  exp: string;
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
  sub: string;
}
export interface Dormitory {
  id: number;
  name: string;
  address: string;
  roomCapacity: number;
  floors: DormFloor[];
}

export interface DormFloor {
  id: number;
  number: number;
}

export interface Room {
  id: number;
  number: number;
  area: number;
  numberOfLivingPlaces: number;
}

export enum RequestTypes {
  None,
  MoveIn,
  MoveOut,
  ChangeRoom,
}

export interface UserRequest {
  id: number;
  requestType: RequestTypes;
  createdAt: Date;
  description: string;
}
