import * as React from "react";

export class UrlManager {
  static _serverUrl = "https://localhost:7141/api/";

  /** Auth */

  public static getLoginEndpoint() {
    return `${UrlManager._serverUrl}login`;
  }

  public static getRegisterEndpoint() {
    return `${UrlManager._serverUrl}register`;
  }

  /** Dorm controller endpoints */
  private static _DormitoriesController: string = `${UrlManager._serverUrl}Dormitories/`;

  public static getAllDormsEndpoint() {
    return `${UrlManager._DormitoriesController}`;
  }

  /** Room controller endpoints */
  private static _RoomsController: string = `${UrlManager._serverUrl}Rooms/`;

  public static getFreeRoomsEndpoint(dormId: number) {
    return `${UrlManager._RoomsController}GetFreeRooms?dormId=${dormId}`;
  }

  public static geAllRoomsEndpoint() {
    return `${UrlManager._RoomsController}`;
  }
}
