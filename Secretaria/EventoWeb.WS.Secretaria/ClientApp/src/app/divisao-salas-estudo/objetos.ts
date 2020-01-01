import { DTOBasicoInscricao } from '../inscricao/objetos';

export class DTODivisaoSalaEstudo {
  Id: number;
  Nome: string;
  Coordenadores: DTOBasicoInscricao[];
  Participantes: DTOBasicoInscricao[]
}
