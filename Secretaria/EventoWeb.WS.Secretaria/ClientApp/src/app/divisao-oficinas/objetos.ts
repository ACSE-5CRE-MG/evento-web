import { DTOBasicoInscricao } from '../inscricao/objetos';

export class DTODivisaoOficina {
  Id: number;
  Nome: string;
  Coordenadores: DTOBasicoInscricao[];
  Participantes: DTOBasicoInscricao[]
}
